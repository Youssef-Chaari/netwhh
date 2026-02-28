import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';
import { ApiService } from '../../core/services/api.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, BaseChartDirective],
  template: `
    <div class="space-y-8 animate-fade-in transition-all duration-500 pb-20">
      
      <!-- HEADER -->
      <div class="flex items-center justify-between border-b border-gray-800 pb-4">
        <div>
          <h2 class="text-3xl font-bold font-mono text-white tracking-widest"><span class="neon-text-cyan">OVERVIEW</span>_NEXUS</h2>
          <p class="text-[var(--color-neon-magenta)] text-xs font-mono tracking-widest uppercase mt-1">Live Telemetry Data Feed</p>
        </div>
      </div>

      <!-- GLOWING KPI GRID -->
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        
        <!-- KPI 1 -->
        <div class="glass-panel p-6 relative overflow-hidden group">
          <div class="absolute top-0 right-0 w-16 h-16 bg-[var(--color-neon-cyan)] rounded-bl-full opacity-10 group-hover:opacity-30 transition-opacity"></div>
          <div class="absolute -left-1 top-4 bottom-4 w-1 bg-[var(--color-neon-cyan)] shadow-[0_0_10px_var(--color-neon-cyan)] animate-pulse"></div>
          
          <h3 class="text-xs font-mono text-gray-400 uppercase tracking-widest mb-1">Gross Revenue</h3>
          <div class="text-3xl font-bold text-white">{{ (kpis?.totalSales || 0) | currency:'USD':'symbol':'1.0-0' }}</div>
        </div>

        <!-- KPI 2 -->
        <div class="glass-panel p-6 relative overflow-hidden group">
          <div class="absolute top-0 right-0 w-16 h-16 bg-[var(--color-neon-magenta)] rounded-bl-full opacity-10 group-hover:opacity-30 transition-opacity"></div>
          <div class="absolute -left-1 top-4 bottom-4 w-1 bg-[var(--color-neon-magenta)] shadow-[0_0_10px_var(--color-neon-magenta)] animate-pulse"></div>
          
          <h3 class="text-xs font-mono text-gray-400 uppercase tracking-widest mb-1">Absolute Profit</h3>
          <div class="text-3xl font-bold text-white">{{ (kpis?.totalGrossMargin || 0) | currency:'USD':'symbol':'1.0-0' }}</div>
        </div>

        <!-- KPI 3 -->
        <div class="glass-panel p-6 relative overflow-hidden group">
          <div class="absolute top-0 right-0 w-16 h-16 bg-[var(--color-neon-cyan)] rounded-bl-full opacity-10 group-hover:opacity-30 transition-opacity"></div>
          <div class="absolute -left-1 top-4 bottom-4 w-1 bg-[var(--color-neon-cyan)] shadow-[0_0_10px_var(--color-neon-cyan)]"></div>
          
          <h3 class="text-xs font-mono text-gray-400 uppercase tracking-widest mb-1">Avg Margin Rate</h3>
          <div class="text-3xl font-bold text-white">{{ (kpis?.averageMarginRate || 0) | number:'1.1-2' }}%</div>
        </div>

        <!-- KPI 4 -->
        <div class="glass-panel p-6 relative overflow-hidden group">
          <div class="absolute top-0 right-0 w-16 h-16 bg-[var(--color-neon-magenta)] rounded-bl-full opacity-10 group-hover:opacity-30 transition-opacity"></div>
          <div class="absolute -left-1 top-4 bottom-4 w-1 bg-[var(--color-neon-magenta)] shadow-[0_0_10px_var(--color-neon-magenta)]"></div>
          
          <h3 class="text-xs font-mono text-gray-400 uppercase tracking-widest mb-1">Avg Order Value</h3>
          <div class="text-3xl font-bold text-white">{{ (kpis?.averageOrderValue || 0) | currency:'USD':'symbol':'1.0-0' }}</div>
        </div>
      </div>

      <!-- CHARTS SECTION -->
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
        
        <!-- PROFITABILITY OVER TIME (COMBO CHART) -->
        <div class="glass-panel p-6">
          <h3 class="text-sm font-mono text-[var(--color-neon-cyan)] uppercase tracking-widest mb-4 flex items-center">
            <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 7h8m0 0v8m0-8l-8 8-4-4-6 6"></path></svg>
            Revenue vs Profit Generation
          </h3>
          <div class="h-[300px] w-full relative">
             <canvas baseChart
               [data]="lineChartData"
               [options]="comboChartOptions"
               [type]="'bar'">
             </canvas>
          </div>
        </div>

        <!-- SALES BY CHANNEL (PIE CHART) -->
        <div class="glass-panel p-6">
          <h3 class="text-sm font-mono text-[var(--color-neon-magenta)] uppercase tracking-widest mb-4 flex items-center">
            <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 3.055A9.001 9.001 0 1020.945 13H11V3.055z"></path><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20.488 9H15V3.512A9.025 9.025 0 0120.488 9z"></path></svg>
            Sales Channel Distribution
          </h3>
          <div class="h-[300px] w-full relative">
             <canvas baseChart
               [data]="pieChartData"
               [options]="pieChartOptions"
               [type]="'doughnut'">
             </canvas>
          </div>
        </div>

        <!-- TOP PRODUCTS (BAR CHART) -->
        <div class="glass-panel p-6 lg:col-span-2">
           <h3 class="text-sm font-mono text-gray-300 uppercase tracking-widest mb-4 flex items-center">
            <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z"></path></svg>
            Product Performance Index
          </h3>
          <div class="h-[400px] w-full relative">
             <canvas baseChart
               [data]="barChartData"
               [options]="barChartOptions"
               [type]="'bar'">
             </canvas>
          </div>
        </div>

        <!-- TOP CLIENTS (BAR CHART) -->
        <div class="glass-panel p-6 lg:col-span-2">
           <h3 class="text-sm font-mono text-[var(--color-neon-cyan)] uppercase tracking-widest mb-4 flex items-center">
            <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z"></path></svg>
            client_yield_analysis
          </h3>
          <div class="h-[400px] w-full relative">
             <canvas baseChart
               [data]="customerBarChartData"
               [options]="barChartOptions"
               [type]="'bar'">
             </canvas>
          </div>
        </div>
      </div>

      <!-- LISTS SECTION: TOP CUSTOMERS & PRODUCTS -->
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-6 pb-20">
        
        <!-- TOP CUSTOMERS -->
        <div class="glass-panel relative overflow-hidden">
          <div class="bg-black/50 border-b border-gray-800 p-4 flex justify-between items-center">
            <h3 class="text-sm font-mono text-[var(--color-neon-cyan)] uppercase tracking-widest">Elite Clients</h3>
            <span class="text-xs text-gray-500 font-mono">TOP 50</span>
          </div>
          
          <div class="p-0 overflow-y-auto max-h-[500px]">
             <!-- Loading state -->
             <div *ngIf="!topCustomers" class="p-8 flex justify-center">
               <div class="w-6 h-6 border-2 border-[var(--color-neon-cyan)] border-t-transparent rounded-full animate-spin"></div>
             </div>

             <table *ngIf="topCustomers" class="w-full text-sm text-left align-middle border-collapse">
                <tbody>
                  <tr *ngFor="let c of topCustomers; let i = index" class="border-b border-gray-800/50 hover:bg-white/5 transition-colors">
                    <td class="p-4 w-12 text-center">
                      <span class="text-xs font-mono text-gray-500">#{{ i + 1 }}</span>
                    </td>
                    <td class="p-4">
                      <div class="flex items-center gap-3">
                        <div class="w-8 h-8 rounded-full bg-gradient-to-br from-gray-700 to-gray-900 flex items-center justify-center border border-gray-600 shadow-inner">
                          <span class="text-xs font-mono text-white">{{ c.customerName.charAt(0) }}</span>
                        </div>
                        <span class="font-medium text-gray-200">{{ c.customerName }}</span>
                      </div>
                    </td>
                    <td class="p-4 text-right">
                      <div class="font-mono text-[var(--color-neon-cyan)]">{{ c.totalSales | currency:'USD':'symbol':'1.0-0' }}</div>
                      <div class="text-[10px] text-gray-500 tracking-wider">ORD: {{ c.orderCount }}</div>
                    </td>
                  </tr>
                </tbody>
             </table>
          </div>
        </div>

        <!-- TOP PRODUCTS WITH GENERATED PHOTOS -->
        <div class="glass-panel relative overflow-hidden">
          <div class="bg-black/50 border-b border-gray-800 p-4 flex justify-between items-center">
            <h3 class="text-sm font-mono text-[var(--color-neon-magenta)] uppercase tracking-widest">High-Yield Assets</h3>
            <span class="text-xs text-gray-500 font-mono">TOP 50 PRODUCTS</span>
          </div>
          
          <div class="p-0 overflow-y-auto max-h-[500px]">
             <!-- Loading state -->
             <div *ngIf="!topProducts" class="p-8 flex justify-center">
               <div class="w-6 h-6 border-2 border-[var(--color-neon-magenta)] border-t-transparent rounded-full animate-spin"></div>
             </div>
             
             <!-- Products List (Grid Layout) -->
             <div *ngIf="topProducts" class="grid grid-cols-1 sm:grid-cols-2 gap-4 p-4">
                
                <div *ngFor="let p of topProducts; let i = index" class="border border-gray-800 rounded-lg p-3 hover:border-[var(--color-neon-magenta)]/50 bg-black/30 transition-all group flex flex-col h-full relative overflow-hidden">
                  
                  <!-- Rank Badge -->
                  <div class="absolute top-2 right-2 bg-black/80 backdrop-blur border border-gray-700 text-xs font-mono px-2 py-0.5 rounded text-[var(--color-neon-magenta)] z-10 shadow-[0_0_5px_var(--color-neon-magenta)]">
                    #{{ i + 1 }}
                  </div>

                  <h4 class="text-sm font-medium text-gray-200 mb-2 truncate pr-10" [title]="p.productName">{{ p.productName }}</h4>
                  
                  <div class="mt-auto pt-2 flex justify-between items-end border-t border-gray-800">
                    <div>
                      <div class="text-[10px] uppercase text-gray-500 tracking-widest mb-0.5">Abs Profit</div>
                      <div class="text-sm font-mono text-[var(--color-neon-cyan)]">{{ p.totalGrossMargin | currency:'USD':'symbol':'1.0-0' }}</div>
                    </div>
                    <div class="text-right">
                       <div class="text-[10px] uppercase text-gray-500 tracking-widest mb-0.5">Rev</div>
                       <div class="text-sm font-mono text-gray-300">{{ p.totalSales | currency:'USD':'symbol':'1.0-0' }}</div>
                    </div>
                  </div>
                </div>

             </div>
          </div>
        </div>

      </div>
    </div>
  `
})
export class DashboardComponent implements OnInit {
  apiService = inject(ApiService);
  authService = inject(AuthService);
  router = inject(Router);

  kpis: any = null;
  topCustomers: any[] | null = null;
  topProducts: any[] | null = null;
  isAdmin = false;

  // Chart Properties
  public lineChartData: ChartData<'bar' | 'line'> = { labels: [], datasets: [] }; // Now used for Combo Chart (Bar + Line)
  public pieChartData: ChartData<'doughnut'> = { labels: [], datasets: [] };
  public barChartData: ChartData<'bar'> = { labels: [], datasets: [] };
  public customerBarChartData: ChartData<'bar'> = { labels: [], datasets: [] };

  public comboChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: { legend: { labels: { color: '#aaa', font: { family: 'monospace', size: 10 } } } },
    scales: {
      y: { type: 'linear', position: 'left', grid: { color: 'rgba(255, 255, 255, 0.1)' }, ticks: { color: '#666', font: { family: 'monospace' } } },
      x: { grid: { display: false }, ticks: { color: '#666', font: { family: 'monospace' } } }
    }
  };

  public pieChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { position: 'right', labels: { color: '#aaa', font: { family: 'monospace', size: 10 } } }
    }
  };

  public barChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    indexAxis: 'y',
    plugins: { legend: { display: false } },
    scales: {
      x: { grid: { color: 'rgba(255, 255, 255, 0.1)' }, ticks: { color: '#666', font: { family: 'monospace' } } },
      y: { grid: { display: false }, ticks: { color: '#aaa', font: { family: 'monospace' } } }
    }
  };

  ngOnInit() {
    this.isAdmin = this.authService.getRole() === 'Admin';
    this.loadData();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  loadData() {
    this.apiService.getKpis().subscribe(res => this.kpis = res);

    this.apiService.getSalesByCustomer().subscribe((res: any[]) => {
      this.topCustomers = res;
      this.customerBarChartData = {
        labels: res.slice(0, 10).map((c: any) => c.customerName),
        datasets: [{
          data: res.slice(0, 10).map((c: any) => c.totalSales),
          label: 'Revenue by Client',
          backgroundColor: 'rgba(236, 72, 153, 0.5)',
          borderColor: '#ec4899',
          borderWidth: 1,
          hoverBackgroundColor: '#ec4899'
        }]
      };
    });

    this.apiService.getSalesByProduct().subscribe((res: any[]) => {
      this.topProducts = res;
      this.barChartData = {
        labels: res.slice(0, 10).map((p: any) => p.productName),
        datasets: [{
          data: res.slice(0, 10).map((p: any) => p.totalGrossMargin), // Represent Profit
          label: 'Gross Profit',
          backgroundColor: 'rgba(6, 182, 212, 0.5)',
          borderColor: '#06b6d4',
          borderWidth: 1,
          hoverBackgroundColor: '#06b6d4'
        }]
      };
    });

    this.apiService.getSalesByPeriod().subscribe((data: any[]) => {
      this.lineChartData = {
        labels: data.map((d: any) => `${d.year}-${d.month.toString().padStart(2, '0')}`),
        datasets: [
          {
            type: 'bar',
            data: data.map((d: any) => d.totalSales),
            label: 'Revenue',
            backgroundColor: 'rgba(236, 72, 153, 0.3)',
            borderColor: '#ec4899',
            borderWidth: 1,
            order: 2
          },
          {
            type: 'line',
            data: data.map((d: any) => d.totalGrossMargin),
            label: 'Absolute Profit',
            fill: true,
            tension: 0.4,
            borderColor: '#06b6d4',
            backgroundColor: 'rgba(6, 182, 212, 0.1)',
            pointBackgroundColor: '#06b6d4',
            pointBorderColor: '#fff',
            order: 1
          }
        ]
      };
    });

    this.apiService.getSalesByChannel().subscribe((data: any[]) => {
      this.pieChartData = {
        labels: data.map((d: any) => d.channel),
        datasets: [{
          data: data.map((d: any) => d.totalSales),
          backgroundColor: [
            'rgba(6, 182, 212, 0.7)', // Cyan for Online
            'rgba(168, 85, 247, 0.7)', // Purple for Reseller
          ],
          borderColor: 'rgba(0,0,0,0.5)',
          borderWidth: 2
        }]
      };
    });


  }
}
