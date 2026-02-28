import { Component, inject } from '@angular/core';
import { RouterOutlet, Router } from '@angular/router';
import { AuthService } from './core/services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  template: `
    <!-- Futuriste Background Effects (Glassmorphism + Neon) -->
    <div class="fixed inset-0 z-[-1] overflow-hidden pointer-events-none">
      <div class="absolute top-[10%] left-[20%] w-96 h-96 bg-[var(--color-neon-cyan)]/10 rounded-full blur-[120px] mix-blend-screen animate-pulse-glow"></div>
      <div class="absolute bottom-[20%] right-[10%] w-[500px] h-[500px] bg-[var(--color-neon-magenta)]/10 rounded-full blur-[150px] mix-blend-screen"></div>
    </div>

    <!-- Navigation Bar - Only show when authenticated -->
    <nav *ngIf="authService.currentUser$ | async as user" class="glass-panel sticky top-0 z-50 rounded-none border-t-0 border-l-0 border-r-0 mb-6 px-6 py-4 flex justify-between items-center relative overflow-hidden">
      <!-- Glow top border line -->
      <div class="absolute top-0 left-0 w-full h-[1px] bg-gradient-to-r from-transparent via-[var(--color-neon-cyan)] to-transparent opacity-80"></div>
      
      <div class="flex items-center gap-4">
        <h1 class="text-2xl font-bold font-mono tracking-widest text-white">
          <span class="neon-text-cyan">BI</span><span class="opacity-50">_DASHBOARD</span>
        </h1>
        
        <div class="ml-10 flex gap-6">
          <a *ngIf="user.role === 'Admin'" class="text-sm font-semibold tracking-wider uppercase text-gray-300 hover:text-[var(--color-neon-cyan)] transition-colors cursor-pointer" (click)="router.navigate(['/dashboard'])">Analytics</a>
          <a class="text-sm font-semibold tracking-wider uppercase text-gray-300 hover:text-[var(--color-neon-cyan)] transition-colors cursor-pointer" (click)="router.navigate(['/products'])">Products</a>
          <a *ngIf="user.role !== 'Admin'" class="text-sm font-semibold tracking-wider uppercase text-gray-300 hover:text-[var(--color-neon-cyan)] transition-colors cursor-pointer" (click)="router.navigate(['/my-orders'])">My Orders</a>
          <a *ngIf="user.role === 'Admin'" class="text-sm font-semibold tracking-wider uppercase text-gray-300 hover:text-[var(--color-neon-magenta)] transition-colors cursor-pointer" (click)="router.navigate(['/admin/orders'])">Orders</a>
          <a *ngIf="user.role === 'Admin'" class="text-sm font-semibold tracking-wider uppercase text-gray-300 hover:text-[var(--color-neon-magenta)] transition-colors cursor-pointer" (click)="router.navigate(['/admin/categories'])">Categories</a>
          <a *ngIf="user.role === 'Admin'" class="text-sm font-semibold tracking-wider uppercase text-gray-300 hover:text-[var(--color-neon-magenta)] transition-colors cursor-pointer" (click)="router.navigate(['/admin/users'])">Admin Users</a>
        </div>
      </div>
      
      <div class="flex items-center gap-6">
        <div class="flex flex-col items-end">
          <span class="text-sm text-white font-mono opacity-80">{{ user.username }}</span>
          <span class="text-xs text-[var(--color-neon-cyan)] font-mono tracking-widest uppercase">{{ user.role }}</span>
        </div>
        <button (click)="logout()" class="btn-neon text-xs py-1.5 px-4 border-gray-600 text-gray-400 hover:border-red-500 hover:text-red-500 hover:shadow-[0_0_15px_rgba(239,68,68,0.5)] hover:bg-transparent">
          LOGOUT
        </button>
      </div>
    </nav>

    <!-- Main Content Routing -->
    <main class="container mx-auto px-4 pb-12">
      <router-outlet></router-outlet>
    </main>
  `
})
export class AppComponent {
  authService = inject(AuthService);
  router = inject(Router);

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
