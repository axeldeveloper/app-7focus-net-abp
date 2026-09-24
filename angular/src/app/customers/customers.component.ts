
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { CoreModule, ListService, PagedResultDto, ABP } from '@abp/ng.core';
import { NgbDateNativeAdapter, NgbDropdownModule, NgbDateAdapter, NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';


import { ThemeSharedModule, ConfirmationService, Confirmation, ToasterService } from '@abp/ng.theme.shared';

import { CustomerService, CustomerDto, CreateUpdateCustomerDto } from '../proxy/customers';

@Component({
  selector: 'app-customers',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,  // ← esta linha
  imports: [
    CommonModule, 
    ReactiveFormsModule, 
    NgxDatatableModule, 
    CoreModule, 
    ThemeSharedModule, 
    NgbDatepickerModule, 
    NgbDropdownModule,
    ],
  templateUrl: './customers.component.html',
  providers: [
    ListService,  
    { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter },
  ],
})
export class CustomersComponent implements OnInit {
  customers: PagedResultDto<CustomerDto> = { items: [], totalCount: 0 };
  selectedCustomer = {} as CustomerDto; // declare selectedBook

  isModalOpen = false;
  form!: FormGroup;
  selectedCustomerId?: string;

  constructor(
    private confirmation: ConfirmationService,
    private toaster: ToasterService,
  ) {}

  customer = { items: [], totalCount: 0 } as PagedResultDto<CustomerDto>;
  private readonly cdr = inject(ChangeDetectorRef);

  public readonly list = inject(ListService);
  private readonly customerService = inject(CustomerService);
  private readonly fb = inject(FormBuilder); // inject FormBuilder

  ngOnInit() {
    const customerStreamCreator = (query: any) => this.customerService.getList(query);
    this.list.hookToQuery(customerStreamCreator).subscribe(response => {
      this.customer = response;
      this.cdr.markForCheck(); // força o Angular a re-renderizar
    });
  }

  buildForm() {
    this.form = this.fb.group({
      code: [this.selectedCustomer.code || '', Validators.required],
      cpf: [this.selectedCustomer.cpf || '', Validators.required],
      name: [this.selectedCustomer.name || '', [Validators.required, Validators.minLength(3)]],
      //dateOfBirth: [null, Validators.required],
      dateOfBirth: [
        this.selectedCustomer.dateOfBirth ? new Date(this.selectedCustomer.dateOfBirth) : null,
        Validators.required,
      ],
    });
  }

  openCreate() {
    this.selectedCustomer = {} as CustomerDto; // reset the selected book

    this.selectedCustomerId = undefined;
    this.buildForm();
    this.isModalOpen = true;
  }

  openEdit(id: string) {
    this.customerService.get(id).subscribe(customer => {
      this.selectedCustomerId = id;
      this.selectedCustomer = customer; // set the selected customer
      this.buildForm();
      this.form.patchValue({
        ...customer,
        //dateOfBirth: customer.dateOfBirth?.substring(0, 10),
        dateOfBirth: customer.dateOfBirth ? new Date(customer.dateOfBirth) : null,
      });
      this.isModalOpen = true;
    });
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const request: CreateUpdateCustomerDto = this.form.value;

    const action = this.selectedCustomerId
      ? this.customerService.update(this.selectedCustomerId, request)
      : this.customerService.create(request);

    action.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
      this.toaster.success('::SuccessfullySaved', undefined, { life: 3000 });
    });
  }

  delete(id: string) {
    this.confirmation
      .warn('::Action:AreYouSureToDelete', '::AreYouSure')
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.customerService.delete(id).subscribe(() => this.list.get());
        }
      });
  }
}