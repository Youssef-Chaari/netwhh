import { TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { AuthService } from './core/services/auth.service';
import { Router } from '@angular/router';
import { BehaviorSubject, of } from 'rxjs';

describe('AppComponent', () => {
  let mockAuthService: any;
  let mockRouter: any;

  beforeEach(async () => {
    // Création d'un mock avec un observable pour éviter les crash pipe async 
    mockAuthService = {
      currentUser$: of({ username: 'Test', role: 'User' }),
      logout: jasmine.createSpy('logout')
    };

    mockRouter = {
      navigate: jasmine.createSpy('navigate')
    };

    await TestBed.configureTestingModule({
      imports: [AppComponent],
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter }
      ]
    }).compileComponents();
  });

  it('devrait créer l app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  // Suppression des tests inutiles générés sur "title" et "render h1" qui ne 
  // correspondent plus au vrai code html de app.component.
});
