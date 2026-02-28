import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../core/services/api.service';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="min-h-screen bg-[var(--color-bg)] text-white px-6 pb-12">
      <div class="max-w-4xl mx-auto flex flex-col gap-6">

        <!-- HEADER -->
        <div class="border-b border-gray-800/60 pb-5 pt-2">
          <div class="flex items-center gap-3 mb-1">
            <div class="w-1 h-8 bg-[var(--color-neon-cyan)] shadow-[0_0_10px_var(--color-neon-cyan)]"></div>
            <h2 class="text-3xl font-bold font-mono text-white tracking-widest">
              <span class="neon-text-cyan">MY</span>_ORDERS
            </h2>
          </div>
          <p class="text-[var(--color-neon-magenta)] text-xs font-mono tracking-widest uppercase ml-4">
            Historique de vos commandes
          </p>
        </div>

        <!-- SUMMARY STATS -->
        <div *ngIf="!loading && orders.length > 0" class="grid grid-cols-3 gap-4">
          <div class="glass-panel p-4 flex flex-col gap-1 border-l-2 border-l-yellow-500">
            <p class="text-[10px] font-mono text-gray-500 tracking-widest uppercase">En Attente</p>
            <p class="text-2xl font-bold font-mono text-yellow-400">{{ countByStatus('Pending') }}</p>
          </div>
          <div class="glass-panel p-4 flex flex-col gap-1 border-l-2 border-l-green-500">
            <p class="text-[10px] font-mono text-gray-500 tracking-widest uppercase">Confirmées</p>
            <p class="text-2xl font-bold font-mono text-green-400">{{ countByStatus('Confirmed') }}</p>
          </div>
          <div class="glass-panel p-4 flex flex-col gap-1 border-l-2 border-l-[var(--color-neon-cyan)]">
            <p class="text-[10px] font-mono text-gray-500 tracking-widest uppercase">Total Dépensé</p>
            <p class="text-2xl font-bold font-mono text-[var(--color-neon-cyan)]">{{ totalSpent() | number:'1.2-2' }} €</p>
          </div>
        </div>

        <!-- LOADING -->
        <div *ngIf="loading" class="flex items-center gap-3 text-[var(--color-neon-cyan)] font-mono text-sm py-12 justify-center">
          <div class="w-5 h-5 border-2 border-[var(--color-neon-cyan)] border-t-transparent rounded-full animate-spin"></div>
          Chargement des commandes...
        </div>

        <!-- EMPTY -->
        <div *ngIf="!loading && orders.length === 0"
             class="glass-panel p-16 text-center text-gray-500 font-mono flex flex-col items-center gap-4">
          <div class="w-16 h-16 border border-gray-700 rounded-full flex items-center justify-center text-3xl text-gray-600">📦</div>
          <div>
            <p class="text-lg text-gray-400">Aucune commande trouvée</p>
            <p class="text-xs mt-2 text-gray-600">Passez votre première commande depuis le catalogue de produits.</p>
          </div>
        </div>

        <!-- ORDER CARDS -->
        <div *ngFor="let order of orders; let i = index"
             class="glass-panel relative overflow-hidden border border-gray-800/50 hover:border-gray-700 transition-all duration-200">

          <!-- Left status bar -->
          <div [ngClass]="statusBarClass(order.status)"
               class="absolute left-0 top-0 bottom-0 w-1"></div>

          <div class="pl-5 pr-5 pt-4 pb-5">
            <!-- Order header -->
            <div class="flex items-start justify-between mb-4">
              <div class="flex items-center gap-4">
                <div>
                  <div class="flex items-center gap-2">
                    <span class="text-[var(--color-neon-magenta)] font-mono font-bold text-sm tracking-widest">#{{ order.id }}</span>
                    <span class="text-gray-600 text-xs font-mono">|</span>
                    <span class="text-gray-400 text-xs font-mono">{{ order.orderDate | date: 'dd MMM yyyy · HH:mm' }}</span>
                  </div>
                  <p class="text-xs text-gray-600 font-mono mt-0.5">{{ order.items?.length ?? 0 }} article(s)</p>
                </div>
              </div>
              <span [ngClass]="statusBadgeClass(order.status)"
                    class="px-3 py-1 text-xs font-mono font-bold tracking-widest rounded-sm border flex items-center gap-1.5">
                <span class="w-1.5 h-1.5 rounded-full" [ngClass]="statusDotClass(order.status)"></span>
                {{ order.status | uppercase }}
              </span>
            </div>

            <!-- Divider -->
            <div class="border-t border-gray-800/60 mb-3"></div>

            <!-- Items list -->
            <div class="space-y-1.5 mb-4">
              <div *ngFor="let item of order.items"
                   class="flex justify-between items-center text-sm py-2 px-3 rounded bg-white/[0.02] border border-gray-800/40">
                <div class="flex items-center gap-3">
                  <span class="w-6 h-6 rounded bg-[var(--color-neon-magenta)]/10 border border-[var(--color-neon-magenta)]/30 flex items-center justify-center text-[var(--color-neon-magenta)] font-mono font-bold text-xs">
                    {{ item.quantity }}
                  </span>
                  <span class="text-gray-200 font-medium">{{ item.productName }}</span>
                </div>
                <div class="text-right">
                  <span class="font-mono text-gray-300 text-sm">{{ (item.unitPrice * item.quantity) | number:'1.2-2' }} €</span>
                  <p class="text-[10px] text-gray-600 font-mono">{{ item.unitPrice | number:'1.2-2' }} € / u.</p>
                </div>
              </div>
            </div>

            <!-- Total -->
            <div class="flex justify-end items-center gap-3">
              <span class="text-gray-600 text-xs font-mono tracking-widest uppercase">Total commande</span>
              <span class="text-xl font-bold font-mono neon-text-cyan">{{ order.totalAmount | number:'1.2-2' }} €</span>
            </div>
          </div>
        </div>

      </div>
    </div>
  `
})
export class OrdersComponent implements OnInit {
  private apiService = inject(ApiService);

  orders: any[] = [];
  loading = true;

  ngOnInit() {
    this.apiService.getMyOrders().subscribe({
      next: (data) => { this.orders = data; this.loading = false; },
      error: () => { this.loading = false; }
    });
  }

  countByStatus(status: string): number {
    return this.orders.filter(o => o.status === status).length;
  }

  totalSpent(): number {
    return this.orders
      .filter(o => o.status !== 'Canceled')
      .reduce((sum, o) => sum + o.totalAmount, 0);
  }

  statusBarClass(status: string): string {
    switch (status?.toLowerCase()) {
      case 'confirmed': return 'bg-green-500 shadow-[0_0_8px_rgba(34,197,94,0.6)]';
      case 'canceled': return 'bg-red-500';
      default: return 'bg-yellow-500 shadow-[0_0_8px_rgba(234,179,8,0.6)]';
    }
  }

  statusBadgeClass(status: string): string {
    switch (status?.toLowerCase()) {
      case 'confirmed': return 'border-green-500/50 text-green-400 bg-green-500/10';
      case 'canceled': return 'border-red-500/50 text-red-400 bg-red-500/10';
      default: return 'border-yellow-500/50 text-yellow-400 bg-yellow-500/10';
    }
  }

  statusDotClass(status: string): string {
    switch (status?.toLowerCase()) {
      case 'confirmed': return 'bg-green-400 animate-pulse';
      case 'canceled': return 'bg-red-400';
      default: return 'bg-yellow-400 animate-pulse';
    }
  }
}
