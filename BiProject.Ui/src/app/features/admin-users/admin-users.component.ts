import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ApiService } from '../../core/services/api.service';
import { AuthService } from '../../core/services/auth.service';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-admin-users',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './admin-users.component.html',
  styleUrls: ['./admin-users.component.css']
})
export class AdminUsersComponent implements OnInit {
  private apiService = inject(ApiService);
  private authService = inject(AuthService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  users: any[] = [];
  loading = true;
  showForm = false;
  submitting = false;
  editingUserId: number | null = null;
  adminUsername: string | null = '';

  userForm: FormGroup;

  constructor() {
    this.adminUsername = this.authService.getUsername();
    this.userForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      password: [''], // Required for creation, optional for update
      roleName: ['User', Validators.required]
    });
  }

  ngOnInit() {
    this.loadUsers();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  loadUsers() {
    this.loading = true;
    this.apiService.getAdminUsers().subscribe({
      next: (data) => {
        this.users = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load users', err);
        this.loading = false;
      }
    });
  }

  openCreateForm() {
    this.showForm = true;
    this.editingUserId = null;
    this.userForm.reset({ roleName: 'User' });
    this.userForm.get('password')?.setValidators([Validators.required]);
    this.userForm.get('password')?.updateValueAndValidity();
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  openEditForm(user: any) {
    this.showForm = true;
    this.editingUserId = user.id;
    this.userForm.patchValue({
      username: user.username,
      email: user.email,
      firstName: user.firstName,
      lastName: user.lastName,
      roleName: user.role
    });
    this.userForm.get('password')?.clearValidators();
    this.userForm.get('password')?.updateValueAndValidity();
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  deleteUser(id: number) {
    if (confirm('CRITICAL ACTION: Are you sure you want to completely purge this user?')) {
      this.apiService.deleteAdminUser(id).subscribe({
        next: () => {
          this.loadUsers();
        },
        error: (err) => {
          console.error('Delete failed:', err);
          alert(err.error?.message || 'Failed to delete user.');
        }
      });
    }
  }

  onSubmit() {
    if (this.userForm.invalid) return;

    this.submitting = true;
    const formData = this.userForm.value;

    const request$ = this.editingUserId
      ? this.apiService.updateAdminUser(this.editingUserId, formData)
      : this.apiService.createAdminUser(formData);

    request$.subscribe({
      next: () => {
        this.submitting = false;
        this.showForm = false;
        this.editingUserId = null;
        this.userForm.reset();
        this.loadUsers();
      },
      error: (err) => {
        console.error('Save failed:', err);
        this.submitting = false;
        alert(err.error?.message || 'Failed to save the user.');
      }
    });
  }
}
