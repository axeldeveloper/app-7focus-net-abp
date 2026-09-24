import type { FullAuditedEntityDto } from '@abp/ng.core';

export interface CreateUpdateCustomerDto {
  code: string;
  cpf: string;
  name: string;
  dateOfBirth: string;
}

export interface CustomerDto extends FullAuditedEntityDto<string> {
  code?: string;
  cpf?: string;
  name?: string;
  dateOfBirth?: string;
}
