import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from '../../core/services/api.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
    selector: 'app-products',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './products.component.html',
    styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
    private apiService = inject(ApiService);
    private authService = inject(AuthService);
    private router = inject(Router);
    private fb = inject(FormBuilder);

    products: any[] = [];
    categories: any[] = [];

    loading = true;
    error = '';

    isAdmin = false;
    showForm = false;
    editingId: number | null = null;
    productForm: FormGroup;

    constructor() {
        this.productForm = this.fb.group({
            name: ['', Validators.required],
            productCode: ['', Validators.required],
            price: [0, [Validators.required, Validators.min(0.01)]],
            categoryId: [1, Validators.required],
            description: ['']
        });
    }

    cart: any[] = [];
    orderStatus = '';


    ngOnInit() {
        this.isAdmin = this.authService.getRole() === 'Admin';
        this.loadCategories();
        this.loadProducts();
    }

    logout() {
        this.authService.logout();
        this.router.navigate(['/login']);
    }

    loadProducts() {
        this.loading = true;
        this.apiService.getProducts().subscribe({
            next: (data) => {
                this.products = data;
                this.loading = false;
            },
            error: (err) => {
                this.error = 'Communication node offline. Cannot fetch catalog.';
                this.loading = false;
                console.error('Error fetching products', err);
            }
        });
    }

    loadCategories() {
        this.apiService.getCategories().subscribe({
            next: (data) => { this.categories = data; },
            error: (err) => console.error('Failed to load categories', err)
        });
    }

    toggleForm(product?: any) {
        if (!this.isAdmin) return;

        if (product) {
            this.editingId = product.id;
            const catMatch = this.categories.find(c => c.name === product.categoryName);

            this.productForm.patchValue({
                name: product.name,
                productCode: product.productCode,
                price: product.price,
                categoryId: catMatch ? catMatch.id : (this.categories[0]?.id ?? 1),
                description: product.description || ''
            });
            this.showForm = true;
        } else {
            this.editingId = null;
            this.productForm.reset({ categoryId: 1, description: '' });
            this.showForm = !this.showForm;
        }
    }

    onSubmit() {
        if (this.productForm.invalid || !this.isAdmin) {
            return;
        }

        const formData = this.productForm.value;

        if (this.editingId) {
            this.apiService.updateProduct(this.editingId, formData).subscribe({
                next: () => {
                    this.loadProducts();
                    this.toggleForm();
                },
                error: (err) => {
                    this.error = 'Failed to update protocol.';
                    console.error('Update error', err);
                }
            });
        } else {
            this.apiService.createProduct(formData).subscribe({
                next: () => {
                    this.loadProducts();
                    this.toggleForm();
                },
                error: (err) => {
                    this.error = 'Failed to initialize entity.';
                    console.error('Create error', err);
                }
            });
        }
    }

    deleteProduct(id: number) {
        if (!this.isAdmin) return;

        if (confirm('WARNING: Irreversible data purge. Proceed?')) {
            this.apiService.deleteProduct(id).subscribe({
                next: () => {
                    this.loadProducts();
                },
                error: (err) => {
                    this.error = 'Purge failed. Connection unstable.';
                    console.error('Delete error', err);
                }
            });
        }
    }

    addToOrder(product: any) {
        this.cart.push({
            productId: product.id,
            productName: product.name,
            quantity: 1,
            unitPrice: product.price
        });
        this.orderStatus = `[ ${product.name} ] SECURED IN CART`;
        setTimeout(() => this.orderStatus = '', 2000);
    }

    get cartTotal() {
        return this.cart.reduce((total, item) => total + (item.unitPrice * item.quantity), 0);
    }

    removeFromCart(index: number) {
        this.cart.splice(index, 1);
    }

    checkout() {
        if (!this.cart.length) return;

        const userId = this.authService.getUserId();
        if (!userId) {
            this.error = "Could not authenticate user ID for order transmission.";
            return;
        }

        const orderData = {
            userId: userId,
            items: this.cart.map(item => ({
                productId: item.productId,
                quantity: item.quantity
            }))
        };

        this.apiService.createOrder(orderData).subscribe({
            next: () => {
                this.cart = [];
                this.orderStatus = 'TRANSMISSION COMPLETE. ORDER LOGGED.';
                setTimeout(() => this.orderStatus = '', 5000);
            },
            error: (err) => {
                this.error = 'Order transmission failed.';
                console.error(err);
            }
        });
    }
}
