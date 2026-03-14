import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ReactiveFormsModule } from '@angular/forms';

describe('LoginComponent - Frontend Unit Tests', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        LoginComponent, 
        HttpClientTestingModule, 
        RouterTestingModule, 
        ReactiveFormsModule
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('devrait créer le composant de login', () => {
    expect(component).toBeTruthy();
  });

  it('devrait invalider le formulaire si les champs sont vides', () => {
    component.loginForm.controls['username'].setValue('');
    component.loginForm.controls['password'].setValue('');
    expect(component.loginForm.valid).toBeFalsy();
  });

  it('devrait valider le formulaire si les données sont correctes', () => {
    component.loginForm.controls['username'].setValue('testuservalid');
    component.loginForm.controls['password'].setValue('StrongPassword123!');
    expect(component.loginForm.valid).toBeTruthy();
  });

  it('devrait invalider si manque une info', () => {
    component.loginForm.controls['username'].setValue('');
    component.loginForm.controls['password'].setValue('ValidPwd1!');
    expect(component.loginForm.valid).toBeFalsy();
    expect(component.loginForm.controls['username'].hasError('required')).toBeTruthy();
  });
});
