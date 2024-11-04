import { Component, LOCALE_ID } from '@angular/core';
import { CommonModule, registerLocaleData } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { CurrencyPipe } from '@angular/common';
import { CdbService } from './services/cdb.service';
import localePt from '@angular/common/locales/pt';

registerLocaleData(localePt, 'pt-BR');

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    CurrencyPipe
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers: [
    { provide: LOCALE_ID, useValue: 'pt-BR' }
  ]
})

export class AppComponent {
  calculatorForm: FormGroup;
  results$ = this.calculatorService.results$;

  constructor(
    private fb: FormBuilder,
    private calculatorService: CdbService
  ) {
    this.calculatorForm = this.fb.group({
    value: ['', [Validators.required, Validators.min(0.01), Validators.pattern('^\\d+(\\.\\d{1,2})?$')]], 
      months: ['', [Validators.required, Validators.min(1), Validators.pattern(/^[1-9]\d*$/)]]
    });
  }

  calculate() {
    if (this.calculatorForm.valid) {
      const { value, months } = this.calculatorForm.value;
      this.calculatorService.calculate(value, months);
    }
  }
}