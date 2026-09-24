FROM node:24-alpine AS build
WORKDIR /app

COPY package.json package-lock.json ./
RUN npm ci --legacy-peer-deps

COPY . .
RUN npm run build:prod

FROM nginx:alpine AS final
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/BookStore/browser /usr/share/nginx/html
COPY dynamic-env.json /usr/share/nginx/html/dynamic-env.json
EXPOSE 80