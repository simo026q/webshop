import { environment } from '../core/environments/environment';
import { Injectable } from '@angular/core';
import {HttpClient, HttpParams, HttpResponse} from '@angular/common/http';
import {Order, OrderStatus} from '../interfaces/responses/Order';
import { Observable } from 'rxjs';
import {OrderRequest} from "../interfaces/requests/OrderRequest";

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private readonly apiEndpoint = environment.apiUrl + '/orders';

  constructor(private http: HttpClient) {}

  getAll(page: number, size: number): Observable<HttpResponse<Order[]>> {
    let params = new HttpParams()
      .set('page', page)
      .set('size', size);

    return this.http.get<Order[]>(this.apiEndpoint, { params, observe: 'response' });
  }

  create(orderRequest: OrderRequest): Observable<Order> {
    return this.http.post<Order>(this.apiEndpoint, orderRequest);
  }

  getByOrderId(orderId: string): Observable<Order> {
    return this.http.get<Order>(this.apiEndpoint + '/' + orderId);
  }

  changeStatus(orderId: string, orderStatus: OrderStatus): Observable<Order> {
    const status = Number(orderStatus);
    return this.http.patch<Order>(this.apiEndpoint + '/' + orderId + '/status', {status});
  }

  getByIdForUser(orderId: string): Observable<Order> {
    return this.http.get<Order>(this.apiEndpoint + '/user/' + orderId);
  }

  getAllByUser(page: number, size: number): Observable<HttpResponse<Order[]>> {
    let params = new HttpParams()
      .set('page', page)
      .set('size', size);

    return this.http.get<Order[]>(this.apiEndpoint + '/user', { params, observe: 'response' });
  }
}
