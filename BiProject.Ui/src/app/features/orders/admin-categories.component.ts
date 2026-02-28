import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';

@Component({
  selector: 'app-admin-categories',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  template: `
    <div class="min-h-screen bg-[var(--color-bg)] text-white px-6 pb-12">
      <div class="max-w-3xl mx-auto flex flex-col gap-6">

        <!-- HEADER -->
        <div class="border-b border-gray-800/60 pb-5 pt-2 flex justify-between items-end">
          <div>
            <div class="flex items-center gap-3 mb-1">
              <div class="w-1 h-8 bg-[var(--color-neon-cyan)] shadow-[0_0_10px_var(--color-neon-cyan)]"></div>
              <h2 class="text-3xl font-bold font-mono text-white tracking-widest">
                <span class="neon-text-cyan">PROD</span>_CATEGORIES
              </h2>
            </div>
            <p class="text-[var(--color-neon-magenta)] text-xs font-mono tracking-widest uppercase ml-4">
              {{ categories.length }} catégorie(s) configurée(s)
            </p>
          </div>
          <button (click)="openForm()"
                  class="btn-neon flex items-center gap-2 text-sm py-2">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2.5"
                 stroke="currentColor" class="w-3.5 h-3.5">
              <path stroke-linecap="round" stroke-linejoin="round" d="M12 4.5v15m7.5-7.5h-15"/>
            </svg>
            NOUVELLE
          </button>
        </div>

        <!-- ALERTS -->
        <div *ngIf="successMsg"
             class="bg-green-500/10 border border-green-500/50 text-green-400 px-4 py-3 font-mono text-sm rounded flex items-center gap-2">
          <span>✓</span> {{ successMsg }}
        </div>
        <div *ngIf="errorMsg"
             class="bg-red-500/10 border border-red-500/50 text-red-400 px-4 py-3 font-mono text-sm rounded flex items-center gap-2">
          <span>⚠</span> {{ errorMsg }}
        </div>

        <!-- FORM PANEL -->
        <div *ngIf="showForm"
             class="glass-panel p-5 border-l-4 border-l-[var(--color-neon-cyan)] bg-black/20">
          <h3 class="text-xs font-mono text-[var(--color-neon-cyan)] uppercase tracking-widest mb-4 flex items-center gap-2">
            <span class="w-4 h-[1px] bg-[var(--color-neon-cyan)]"></span>
            {{ editingId ? 'MODIFIER LA CATÉGORIE' : 'CRÉER UNE CATÉGORIE' }}
          </h3>
          <form [formGroup]="categoryForm" (ngSubmit)="onSubmit()">
            <div class="flex gap-3 items-end">
              <div class="flex flex-col gap-1.5 flex-1">
                <label class="text-[10px] text-[var(--color-neon-magenta)] font-mono tracking-widest uppercase">
                  Nom de la catégorie *
                </label>
                <input type="text" formControlName="name"
                       class="w-full bg-black/50 border border-gray-700 text-white px-3 py-2 text-sm font-mono focus:border-[var(--color-neon-cyan)] focus:outline-none focus:shadow-[0_0_10px_rgba(0,229,255,0.15)] transition-all"
                       placeholder="Ex: Electronics, Textile..." />
              </div>
              <div class="flex gap-2">
                <button type="submit" [disabled]="categoryForm.invalid || submitting"
                        class="btn-neon text-sm py-2 px-4 disabled:opacity-50 disabled:cursor-not-allowed whitespace-nowrap">
                  {{ submitting ? '...' : (editingId ? 'METTRE À JOUR' : 'CRÉER') }}
                </button>
                <button type="button" (click)="cancelForm()"
                        class="px-4 py-2 text-sm border border-gray-700 text-gray-400 hover:border-gray-500 font-mono transition-colors whitespace-nowrap">
                  ✗
                </button>
              </div>
            </div>
          </form>
        </div>

        <!-- LOADING -->
        <div *ngIf="loading" class="flex items-center gap-3 text-[var(--color-neon-cyan)] font-mono text-sm py-12 justify-center">
          <div class="w-5 h-5 border-2 border-[var(--color-neon-cyan)] border-t-transparent rounded-full animate-spin"></div>
          Chargement...
        </div>

        <!-- CATEGORIES LIST -->
        <div *ngIf="!loading" class="glass-panel overflow-hidden border border-gray-800/50">
          <table class="w-full text-sm">
            <thead>
              <tr class="border-b border-gray-800 bg-black/20">
                <th class="px-5 py-3 text-left text-[10px] text-[var(--color-neon-cyan)] tracking-widest font-mono font-bold uppercase w-16">#</th>
                <th class="px-5 py-3 text-left text-[10px] text-[var(--color-neon-cyan)] tracking-widest font-mono font-bold uppercase">Nom</th>
                <th class="px-5 py-3 text-right text-[10px] text-[var(--color-neon-cyan)] tracking-widest font-mono font-bold uppercase">Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let cat of categories; let i = index"
                  class="border-b border-gray-900/80 hover:bg-white/[0.03] transition-colors group"
                  [class.border-l-2]="editingId === cat.id"
                  [class.border-l-cyan-500]="editingId === cat.id">

                <td class="px-5 py-3.5">
                  <span class="text-[var(--color-neon-magenta)] font-mono font-bold text-xs">{{ cat.id }}</span>
                </td>

                <td class="px-5 py-3.5">
                  <div class="flex items-center gap-3">
                    <div class="w-7 h-7 rounded bg-[var(--color-neon-cyan)]/10 border border-[var(--color-neon-cyan)]/20 flex items-center justify-center">
                      <span class="text-[var(--color-neon-cyan)] text-xs font-mono font-bold">{{ cat.name?.charAt(0)?.toUpperCase() }}</span>
                    </div>
                    <span class="text-gray-100 font-medium">{{ cat.name }}</span>
                  </div>
                </td>

                <td class="px-5 py-3.5 text-right">
                  <div class="flex gap-2 justify-end opacity-0 group-hover:opacity-100 transition-opacity">
                    <button (click)="openEditForm(cat)"
                            class="px-3 py-1 text-xs border border-[var(--color-neon-cyan)]/50 text-[var(--color-neon-cyan)] hover:bg-[var(--color-neon-cyan)]/10 hover:border-[var(--color-neon-cyan)] transition-all rounded font-mono">
                      ✎ Modifier
                    </button>
                    <button (click)="deleteCategory(cat)"
                            class="px-3 py-1 text-xs border border-red-600/40 text-red-400 hover:bg-red-600/10 hover:border-red-500 transition-all rounded font-mono">
                      ✗ Supprimer
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>

          <div *ngIf="categories.length === 0"
               class="py-12 text-center text-gray-500 font-mono text-sm">
            Aucune catégorie. Créez-en une première.
          </div>
        </div>

      </div>
    </div>
  `
})
export class AdminCategoriesComponent implements OnInit {
  private apiService = inject(ApiService);
  private fb = inject(FormBuilder);

  categories: any[] = [];
  loading = true;
  showForm = false;
  submitting = false;
  editingId: number | null = null;
  successMsg = '';
  errorMsg = '';

  categoryForm: FormGroup;

  constructor() {
    this.categoryForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]]
    });
  }

  ngOnInit() { this.loadCategories(); }

  loadCategories() {
    this.loading = true;
    this.apiService.getCategories().subscribe({
      next: (data) => { this.categories = data; this.loading = false; },
      error: () => { this.loading = false; }
    });
  }

  openForm() {
    this.showForm = true;
    this.editingId = null;
    this.categoryForm.reset();
  }

  openEditForm(cat: any) {
    this.showForm = true;
    this.editingId = cat.id;
    this.categoryForm.patchValue({ name: cat.name });
  }

  cancelForm() {
    this.showForm = false;
    this.editingId = null;
    this.categoryForm.reset();
  }

  onSubmit() {
    if (this.categoryForm.invalid) return;
    this.submitting = true;
    this.successMsg = '';
    this.errorMsg = '';
    const payload = this.categoryForm.value;

    const obs = this.editingId
      ? this.apiService.updateCategory(this.editingId, payload)
      : this.apiService.createCategory(payload);

    obs.subscribe({
      next: () => {
        this.successMsg = this.editingId ? 'Catégorie mise à jour.' : 'Catégorie créée.';
        this.cancelForm();
        this.loadCategories();
        this.submitting = false;
        setTimeout(() => this.successMsg = '', 3000);
      },
      error: () => { this.errorMsg = 'Erreur lors de l\'opération.'; this.submitting = false; }
    });
  }

  deleteCategory(cat: any) {
    if (!confirm(`Supprimer "${cat.name}" ?`)) return;
    this.successMsg = '';
    this.errorMsg = '';
    this.apiService.deleteCategory(cat.id).subscribe({
      next: () => {
        this.successMsg = `"${cat.name}" supprimée.`;
        this.categories = this.categories.filter(c => c.id !== cat.id);
        setTimeout(() => this.successMsg = '', 3000);
      },
      error: (err) => {
        this.errorMsg = err?.error?.message ?? `Impossible de supprimer "${cat.name}" (produits associés).`;
      }
    });
  }
}
