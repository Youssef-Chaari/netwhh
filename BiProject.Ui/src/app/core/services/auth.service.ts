import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

export interface AuthResponse {
  token: string;
  username: string;
  role: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private tokenKey = 'bi_auth_token';

  private currentUserSubject = new BehaviorSubject<AuthResponse | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor() {
    this.loadToken();
  }

  private loadToken() {
    const token = localStorage.getItem(this.tokenKey);
    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        const isExpired = decoded.exp * 1000 < Date.now();

        if (!isExpired) {
          this.currentUserSubject.next({
            token,
            username: decoded.unique_name || decoded.name || 'User',
            role: decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || decoded.role || 'User'
          });
        } else {
          this.logout();
        }
      } catch {
        this.logout();
      }
    }
  }

  login(credentials: any): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/login`, credentials).pipe(
      tap(response => {
        if (response && response.token) {
          localStorage.setItem(this.tokenKey, response.token);
          this.currentUserSubject.next(response);
        }
      })
    );
  }

  register(userData: any): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/register`, userData).pipe(
      tap(response => {
        if (response && response.token) {
          localStorage.setItem(this.tokenKey, response.token);
          this.currentUserSubject.next(response);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  getRole(): string | null {
    const user = this.currentUserSubject.getValue();
    return user ? user.role : null;
  }

  getUsername(): string | null {
    const user = this.currentUserSubject.getValue();
    return user ? user.username : null;
  }

  getUserId(): number | null {
    const token = this.getToken();
    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        const nameIdKey = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
        const idStr = decoded[nameIdKey] || decoded.nameid || decoded.sub;
        return idStr ? parseInt(idStr, 10) : null;
      } catch {
        return null;
      }
    }
    return null;
  }
}
