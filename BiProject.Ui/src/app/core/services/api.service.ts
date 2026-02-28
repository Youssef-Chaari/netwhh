import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private http = inject(HttpClient);

  // Analytics
  getKpis(): Observable<any> {
    return this.http.get(`${environment.apiUrl}/analytics/kpis`);
  }

  getSalesByPeriod(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/analytics/sales/period`);
  }

  getSalesByCustomer(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/analytics/sales/customer`);
  }

  getSalesByProduct(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/analytics/sales/product`);
  }

  getSalesByCategory(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/analytics/sales/category`);
  }

  getSalesByChannel(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/analytics/sales/channel`);
  }

  // Clients (OLTP)
  getClients(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/clients`);
  }

  createClient(client: any): Observable<any> {
    return this.http.post(`${environment.apiUrl}/clients`, client);
  }

  updateClient(id: number, client: any): Observable<any> {
    return this.http.put(`${environment.apiUrl}/clients/${id}`, client);
  }

  deleteClient(id: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/clients/${id}`);
  }

  // Admin Users
  getAdminUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/admin/users`);
  }

  getAdminUser(id: number): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/admin/users/${id}`);
  }

  createAdminUser(user: any): Observable<any> {
    return this.http.post(`${environment.apiUrl}/admin/users`, user);
  }

  updateAdminUser(id: number, user: any): Observable<any> {
    return this.http.put(`${environment.apiUrl}/admin/users/${id}`, user);
  }

  deleteAdminUser(id: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/admin/users/${id}`);
  }

  // Products
  getProducts(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/products`);
  }

  getProduct(id: number): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/products/${id}`);
  }

  createProduct(product: any): Observable<any> {
    return this.http.post(`${environment.apiUrl}/products`, product);
  }

  updateProduct(id: number, product: any): Observable<any> {
    return this.http.put(`${environment.apiUrl}/products/${id}`, product);
  }

  deleteProduct(id: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/products/${id}`);
  }

  // Orders
  getOrders(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/orders`);
  }

  getMyOrders(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/orders/my-orders`);
  }

  getOrder(id: number): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/orders/${id}`);
  }

  createOrder(order: any): Observable<any> {
    return this.http.post(`${environment.apiUrl}/orders`, order);
  }

  updateOrderStatus(id: number, status: string): Observable<any> {
    return this.http.patch(`${environment.apiUrl}/orders/${id}/status`, { status });
  }

  deleteOrder(id: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/orders/${id}`);
  }

  // Categories
  getCategories(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/categories`);
  }

  createCategory(category: any): Observable<any> {
    return this.http.post(`${environment.apiUrl}/categories`, category);
  }

  updateCategory(id: number, category: any): Observable<any> {
    return this.http.put(`${environment.apiUrl}/categories/${id}`, category);
  }

  deleteCategory(id: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/categories/${id}`);
  }
}
