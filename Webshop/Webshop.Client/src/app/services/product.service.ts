import { environment } from '../core/environments/environment';
import { Injectable } from '@angular/core';
import {HttpClient, HttpParams, HttpResponse} from '@angular/common/http';
import { Product } from '../interfaces/responses/Product';
import { ProductRequest } from '../interfaces/requests/ProductRequest';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private readonly apiEndpoint = environment.apiUrl + '/products';

  constructor(private http: HttpClient) {}

  getAll(page: number, size: number, search?: string, category?: string): Observable<HttpResponse<Product[]>> {
    let params = new HttpParams()
      .set('page', page)
      .set('size', size);

    if (search) params = params.set('search', search);
    if (category) params = params.set('category', category);

    return this.http.get<Product[]>(this.apiEndpoint, { params, observe: 'response' });
  }

  getLatest(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiEndpoint + '/latest');
  }

  get(id: string): Observable<Product> {
    return this.http.get<Product>(this.apiEndpoint + '/' + id);
  }

  create(product: ProductRequest): Observable<Product> {
    return this.http.post<Product>(this.apiEndpoint, product);
  }

  update(product: ProductRequest): Observable<Product> {
    return this.http.put<Product>(this.apiEndpoint + '/' + product.id, product);
  }

  disable(id: string): Observable<Product> {
    return this.http.patch<Product>(this.apiEndpoint + '/' + id + '/disable', {});
  }

  activate(id: string): Observable<Product> {
    return this.http.patch<Product>(this.apiEndpoint + '/' + id + '/activate', {});
  }
}
