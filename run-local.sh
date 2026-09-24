#!/usr/bin/env bash
#
# run-local.sh - roda o BookStore sem Docker (API + Angular localmente).
#
# Pré-requisitos:
#   - .NET 10 SDK        (http://dot.net)
#   - Node.js 20+ e npm
#   - SQL Server rodando em localhost:1433 (senha sa: pf766312!)
#
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
HOST_URL="https://localhost:44388"
ANGULAR_URL="http://localhost:4200"
API_LOG="$ROOT/Logs/backend.log"
NG_LOG="$ROOT/Logs/angular.log"
API_PID=""
NG_PID=""

log()  { printf '\033[1;32m[run-local]\033[0m %s\n' "$*"; }
warn() { printf '\033[1;33m[run-local]\033[0m %s\n' "$*"; }
die()  { printf '\033[1;31m[run-local]\033[0m %s\n' "$*" >&2; exit 1; }

cleanup() {
  [ -n "$NG_PID" ] && kill "$NG_PID" 2>/dev/null || true
  [ -n "$API_PID" ] && kill "$API_PID" 2>/dev/null || true
}

trap cleanup EXIT INT TERM
mkdir -p "$ROOT/Logs"

command -v dotnet >/dev/null || die "dotnet não encontrado. Instale o SDK .NET 10."
command -v node   >/dev/null || die "node não encontrado. Instale o Node.js 20+."
command -v npm    >/dev/null || die "npm não encontrado."
command -v nc     >/dev/null || die "nc (netcat) não encontrado."
command -v curl   >/dev/null || die "curl não encontrado."

log "dotnet SDK: $(dotnet --version)"

# 1. SQL Server
log "Verificando SQL Server em localhost:1433..."
if ! nc -z -w 3 localhost 1433 2>/dev/null; then
  warn "SQL Server não está acessível em localhost:1433."
  warn "Inicie o SQL Server (ou rode 'docker compose up sqlserver') antes deste script."
  exit 1
fi
log "SQL Server OK."

# 2. Certificado HTTPS de desenvolvimento (usado pelo host)
if ! dotnet dev-certs https --check >/dev/null 2>&1; then
  log "Certificado HTTPS de desenvolvimento não encontrado, criando..."
  dotnet dev-certs https
fi

# 3. Criação do banco de dados e seed
log "Aplicando migrações e dados iniciais (DbMigrator)..."
dotnet run --project "$ROOT/src/BookStore.DbMigrator" --configuration Debug
log "Migrações aplicadas."

# 4. Backend (API)
log "Iniciando API em $HOST_URL (logs em $API_LOG)..."
(
  cd "$ROOT/src/BookStore.HttpApi.Host" &&
  ASPNETCORE_ENVIRONMENT=Development \
  dotnet run --configuration Debug --no-launch-profile --urls "$HOST_URL"
) >"$API_LOG" 2>&1 &
API_PID=$!

log "Aguardando API ficar saudável..."
if ! curl -sk --max-time 120 --retry 60 --retry-connrefused --retry-delay 2 -o /dev/null "$HOST_URL/health-status"; then
  die "API não respondeu em $HOST_URL/health-status. Veja $API_LOG."
fi
log "API no ar."

# 5. Frontend (Angular)
cd "$ROOT/angular"
if [ ! -d node_modules ]; then
  log "Instalando dependências do Angular (npm ci)..."
  npm ci --legacy-peer-deps
fi

log "Iniciando Angular em $ANGULAR_URL (logs em $NG_LOG)..."
npm start >"$NG_LOG" 2>&1 &
NG_PID=$!

log "Aguardando Angular ficar disponível..."
if ! curl -s --max-time 120 --retry 60 --retry-connrefused --retry-delay 2 -o /dev/null "$ANGULAR_URL"; then
  die "Angular não respondeu em $ANGULAR_URL. Veja $NG_LOG."
fi
log "Angular no ar."

printf '\n\033[1;36m=====================================================\033[0m\n'
log "Backend : $HOST_URL (Swagger: $HOST_URL/swagger)"
log "Frontend: $ANGULAR_URL"
printf '\nPressione Ctrl+C para encerrar.\n'
printf '\033[1;36m=====================================================\033[0m\n'

wait