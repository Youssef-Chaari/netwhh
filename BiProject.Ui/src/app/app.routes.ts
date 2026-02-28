import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/login.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { AdminUsersComponent } from './features/admin-users/admin-users.component';
import { ProductsComponent } from './features/products/products.component';
import { OrdersComponent } from './features/orders/orders.component';
import { AdminOrdersComponent } from './features/orders/admin-orders.component';
import { AdminCategoriesComponent } from './features/orders/admin-categories.component';
import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';

import { RegisterComponent } from './features/auth/register/register.component';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    {
        path: 'dashboard',
        component: DashboardComponent,
        canActivate: [authGuard, adminGuard]
    },
    {
        path: 'admin/users',
        component: AdminUsersComponent,
        canActivate: [authGuard, adminGuard]
    },
    {
        path: 'admin/orders',
        component: AdminOrdersComponent,
        canActivate: [authGuard, adminGuard]
    },
    {
        path: 'admin/categories',
        component: AdminCategoriesComponent,
        canActivate: [authGuard, adminGuard]
    },
    {
        path: 'products',
        component: ProductsComponent,
        canActivate: [authGuard]
    },
    {
        path: 'my-orders',
        component: OrdersComponent,
        canActivate: [authGuard]
    },
    { path: '**', redirectTo: 'login' }
];
