import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';

@Component({
  selector: 'app-admin-orders',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="min-h-screen bg-[var(--color-bg)] text-white px-6 pb-12">
      <div class="max-w-7xl mx-auto flex flex-col gap-6">

        <!-- HEADER -->
        <div class="border-b border-gray-800/60 pb-5 pt-2 flex items-end justify-between">
          <div>
            <div class="flex items-center gap-3 mb-1">
              <div class="w-1 h-8 bg-[var(--color-neon-magenta)] shadow-[0_0_10px_var(--color-neon-magenta)]"></div>
              <h2 class="text-3xl font-bold font-mono text-white tracking-widest">
                <span class="neon-text-magenta">SYS</span>_ORDERS
              </h2>
            </div>
            <p class="text-[var(--color-neon-cyan)] text-xs font-mono tracking-widest uppercase ml-4">
              Gestion Globale des Commandes — {{ orders.length }} commande(s)
            </p>
          </div>
          <button (click)="loadOrders()" class="btn-neon text-xs py-1.5 px-4 flex items-center gap-2">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-3 h-3">
              <path stroke-linecap="round" stroke-linejoin="round" d="M16.023 9.348h4.992v-.001M2.985 19.644v-4.992m0 0h4.992m-4.993 0 3.181 3.183a8.25 8.25 0 0 0 13.803-3.7"/>
            </svg>
            ACTUALISER
          </button>
        </div>

        <!-- KPI CARDS -->
        <div class="grid grid-cols-4 gap-4">
          <div class="glass-panel p-4 relative overflow-hidden border-l-2 border-l-gray-600">
            <div class="absolute inset-0 bg-gradient-to-br from-white/5 to-transparent pointer-events-none"></div>
            <p class="text-[10px] font-mono text-gray-500 tracking-widest uppercase">Total</p>
            <p class="text-3xl font-bold font-mono text-white mt-1">{{ orders.length }}</p>
          </div>
          <div class="glass-panel p-4 relative overflow-hidden border-l-2 border-l-yellow-500">
            <div class="absolute inset-0 bg-gradient-to-br from-yellow-500/5 to-transparent pointer-events-none"></div>
            <p class="text-[10px] font-mono text-gray-500 tracking-widest uppercase">En Attente</p>
            <p class="text-3xl font-bold font-mono text-yellow-400 mt-1">{{ countByStatus('Pending') }}</p>
          </div>
          <div class="glass-panel p-4 relative overflow-hidden border-l-2 border-l-green-500">
            <div class="absolute inset-0 bg-gradient-to-br from-green-500/5 to-transparent pointer-events-none"></div>
            <p class="text-[10px] font-mono text-gray-500 tracking-widest uppercase">Confirmées</p>
            <p class="text-3xl font-bold font-mono text-green-400 mt-1">{{ countByStatus('Confirmed') }}</p>
          </div>
          <div class="glass-panel p-4 relative overflow-hidden border-l-2 border-l-red-500">
            <div class="absolute inset-0 bg-gradient-to-br from-red-500/5 to-transparent pointer-events-none"></div>
            <p class="text-[10px] font-mono text-gray-500 tracking-widest uppercase">Annulées</p>
            <p class="text-3xl font-bold font-mono text-red-400 mt-1">{{ countByStatus('Canceled') }}</p>
          </div>
        </div>

        <!-- ALERTS -->
        <div *ngIf="successMsg"
             class="bg-green-500/10 border border-green-500/50 text-green-400 px-4 py-3 font-mono text-sm rounded flex items-center gap-2">
          <span class="text-green-400">✓</span> {{ successMsg }}
        </div>
        <div *ngIf="errorMsg"
             class="bg-red-500/10 border border-red-500/50 text-red-400 px-4 py-3 font-mono text-sm rounded flex items-center gap-2">
          <span>⚠</span> {{ errorMsg }}
        </div>

        <!-- LOADING -->
        <div *ngIf="loading" class="flex items-center gap-3 text-[var(--color-neon-cyan)] font-mono text-sm py-12 justify-center">
          <div class="w-5 h-5 border-2 border-[var(--color-neon-cyan)] border-t-transparent rounded-full animate-spin"></div>
          Chargement des commandes...
        </div>

        <!-- ORDERS TABLE -->
        <div *ngIf="!loading && orders.length > 0" class="glass-panel overflow-hidden border border-gray-800/50">
          <table class="w-full text-sm">
            <thead>
              <tr class="border-b border-gray-800 bg-black/20">
                <th class="px-4 py-3 text-left text-[10px] text-[var(--color-neon-cyan)] tracking-widest font-mono font-bold uppercase">#</th>
                <th class="px-4 py-3 text-left text-[10px] text-[var(--color-neon-cyan)] tracking-widest font-mono font-bold uppercase">Client</th>
                <th class="px-4 py-3 text-left text-[10px] text-[var(--color-neon-cyan)] tracking-widest font-mono font-bold uppercase">Date</th>
                <th class="px-4 py-3 text-left text-[10px] text-[var(--color-neon-cyan)] tracking-widest font-mono font-bold uppercase">Articles</th>
                <th class="px-4 py-3 text-right text-[10px] text-[var(--color-neon-cyan)] tracking-widest font-mono font-bold uppercase">Total</th>
                <th class="px-4 py-3 text-center text-[10px] text-[var(--color-neon-cyan)] tracking-widest font-mono font-bold uppercase">Statut</th>
                <th class="px-4 py-3 text-right text-[10px] text-[var(--color-neon-cyan)] tracking-widest font-mono font-bold uppercase">Actions</th>
              </tr>
            </thead>
            <tbody>
              <ng-container *ngFor="let order of orders">
                <tr class="border-b border-gray-900/80 hover:bg-white/[0.03] transition-colors cursor-pointer group"
                    (click)="toggleExpand(order.id)">
                  <!-- ID -->
                  <td class="px-4 py-3">
                    <span class="text-[var(--color-neon-magenta)] font-mono font-bold">#{{ order.id }}</span>
                  </td>
                  <!-- Client -->
                  <td class="px-4 py-3">
                    <span class="text-gray-200 font-medium">{{ order.userName }}</span>
                  </td>
                  <!-- Date -->
                  <td class="px-4 py-3">
                    <span class="text-gray-400 font-mono text-xs">{{ order.orderDate | date: 'dd/MM/yy HH:mm' }}</span>
                  </td>
                  <!-- Items count -->
                  <td class="px-4 py-3">
                    <span class="text-gray-400 font-mono text-xs">{{ order.items?.length ?? 0 }} art.</span>
                    <span class="text-gray-700 font-mono text-xs ml-2 group-hover:text-gray-500 transition-colors">
                      {{ expandedOrderId === order.id ? '▲' : '▼' }}
                    </span>
                  </td>
                  <!-- Total -->
                  <td class="px-4 py-3 text-right">
                    <span class="font-mono font-bold text-white">{{ order.totalAmount | number:'1.2-2' }} €</span>
                  </td>
                  <!-- Status -->
                  <td class="px-4 py-3 text-center">
                    <span [ngClass]="statusBadgeClass(order.status)"
                          class="inline-flex items-center gap-1.5 px-2.5 py-1 text-xs font-mono font-bold tracking-widest rounded-sm border">
                      <span class="w-1.5 h-1.5 rounded-full" [ngClass]="statusDotClass(order.status)"></span>
                      {{ order.status }}
                    </span>
                  </td>
                  <!-- Actions -->
                  <td class="px-4 py-3 text-right" (click)="$event.stopPropagation()">
                    <div class="flex gap-1.5 justify-end">
                      <button *ngIf="order.status !== 'Confirmed'"
                              (click)="updateStatus(order, 'Confirmed')"
                              class="px-2.5 py-1 border border-green-600/50 text-green-400 hover:bg-green-600/20 hover:border-green-500 transition-all rounded text-xs font-mono">
                        Confirmer
                      </button>
                      <button *ngIf="order.status !== 'Canceled'"
                              (click)="updateStatus(order, 'Canceled')"
                              class="px-2.5 py-1 border border-red-600/50 text-red-400 hover:bg-red-600/20 hover:border-red-500 transition-all rounded text-xs font-mono">
                        Annuler
                      </button>
                      <button *ngIf="order.status !== 'Pending'"
                              (click)="updateStatus(order, 'Pending')"
                              class="px-2.5 py-1 border border-yellow-600/50 text-yellow-400 hover:bg-yellow-600/20 hover:border-yellow-500 transition-all rounded text-xs font-mono">
                        Pending
                      </button>
                    </div>
                  </td>
                </tr>

                <!-- Expanded detail row -->
                <tr *ngIf="expandedOrderId === order.id">
                  <td colspan="7" class="bg-black/30 border-b border-gray-800/50">
                    <div class="px-8 py-4">
                      <p class="text-[10px] text-[var(--color-neon-cyan)] tracking-widest font-mono mb-3">
                        ── DÉTAIL COMMANDE #{{ order.id }} ──
                      </p>
                      <div class="space-y-1.5">
                        <div *ngFor="let item of order.items"
                             class="flex justify-between items-center text-xs py-2 px-3 rounded bg-white/[0.02] border border-gray-800/40">
                          <div class="flex items-center gap-3">
                            <span class="w-5 h-5 rounded bg-[var(--color-neon-magenta)]/10 border border-[var(--color-neon-magenta)]/30 flex items-center justify-center text-[var(--color-neon-magenta)] font-mono font-bold text-[10px]">
                              {{ item.quantity }}
                            </span>
                            <span class="text-gray-300">{{ item.productName }}</span>
                          </div>
                          <div class="flex gap-6 text-right font-mono">
                            <span class="text-gray-500">{{ item.unitPrice | number:'1.2-2' }} € / u.</span>
                            <span class="text-gray-200 font-bold">{{ (item.quantity * item.unitPrice) | number:'1.2-2' }} €</span>
                          </div>
                        </div>
                      </div>
                    </div>
                  </td>
                </tr>
              </ng-container>
            </tbody>
          </table>
        </div>

        <div *ngIf="!loading && orders.length === 0"
             class="glass-panel p-16 text-center font-mono flex flex-col items-center gap-3 text-gray-500">
          <div class="text-4xl">📋</div>
          <p class="text-gray-400">Aucune commande dans le système.</p>
        </div>

      </div>
    </div>
  `
})
export class AdminOrdersComponent implements OnInit {
  private apiService = inject(ApiService);

  orders: any[] = [];
  loading = true;
  expandedOrderId: number | null = null;
  successMsg = '';
  errorMsg = '';

  ngOnInit() { this.loadOrders(); }

  loadOrders() {
    this.loading = true;
    this.apiService.getOrders().subscribe({
      next: (data) => { this.orders = data; this.loading = false; },
      error: () => { this.loading = false; }
    });
  }

  toggleExpand(id: number) {
    this.expandedOrderId = this.expandedOrderId === id ? null : id;
  }

  countByStatus(status: string): number {
    return this.orders.filter(o => o.status === status).length;
  }

  updateStatus(order: any, status: string) {
    this.successMsg = '';
    this.errorMsg = '';
    this.apiService.updateOrderStatus(order.id, status).subscribe({
      next: () => {
        order.status = status;
        this.successMsg = `Commande #${order.id} → ${status}`;
        setTimeout(() => this.successMsg = '', 3000);
      },
      error: () => { this.errorMsg = `Erreur lors de la mise à jour de #${order.id}`; }
    });
  }

  statusBadgeClass(s: string): string {
    switch (s?.toLowerCase()) {
      case 'confirmed': return 'border-green-500/50 text-green-400 bg-green-500/10';
      case 'canceled': return 'border-red-500/50 text-red-400 bg-red-500/10';
      default: return 'border-yellow-500/50 text-yellow-400 bg-yellow-500/10';
    }
  }

  statusDotClass(s: string): string {
    switch (s?.toLowerCase()) {
      case 'confirmed': return 'bg-green-400 animate-pulse';
      case 'canceled': return 'bg-red-400';
      default: return 'bg-yellow-400 animate-pulse';
    }
  }
}
