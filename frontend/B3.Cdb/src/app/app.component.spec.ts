import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { CdbService } from './services/cdb.service';
import { of } from 'rxjs';

class MockCdbService {
  results$ = of([]);
  calculate(value: number, months: number) {
  }
}

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let calculatorService: CdbService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule],
      declarations: [AppComponent],
      providers: [{ provide: CdbService, useClass: MockCdbService }]
    }).compileComponents();

    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    calculatorService = TestBed.inject(CdbService);
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with empty values', () => {
    expect(component.calculatorForm.value).toEqual({ value: '', months: '' });
  });

  it('should make the value control required', () => {
    const valueControl = component.calculatorForm.get('value');
    valueControl?.setValue('');
    expect(valueControl?.valid).toBeFalsy();
  });

  it('should make the months control required', () => {
    const monthsControl = component.calculatorForm.get('months');
    monthsControl?.setValue('');
    expect(monthsControl?.valid).toBeFalsy();
  });

  it('should validate the value control min value', () => {
    const valueControl = component.calculatorForm.get('value');
    valueControl?.setValue(0);
    expect(valueControl?.valid).toBeFalsy();
    valueControl?.setValue(0.01);
    expect(valueControl?.valid).toBeTruthy();
  });

  it('should validate the months control min value', () => {
    const monthsControl = component.calculatorForm.get('months');
    monthsControl?.setValue(0);
    expect(monthsControl?.valid).toBeFalsy();
    monthsControl?.setValue(1);
    expect(monthsControl?.valid).toBeTruthy();
  });

  it('should call calculate method of calculatorService when form is valid', () => {
    spyOn(calculatorService, 'calculate');
    component.calculatorForm.setValue({ value: 100, months: 12 });
    component.calculate();
    expect(calculatorService.calculate).toHaveBeenCalledWith(100, 12);
  });

  it('should not call calculate method of calculatorService when form is invalid', () => {
    spyOn(calculatorService, 'calculate');
    component.calculatorForm.setValue({ value: '', months: '' });
    component.calculate();
    expect(calculatorService.calculate).not.toHaveBeenCalled();
  });
});
