import { environment } from '../core/environments/environment';
import { Injectable } from '@angular/core';
import {HttpClient, HttpParams, HttpResponse} from '@angular/common/http';
import { User } from '../interfaces/responses/User';
import { UserRequest } from '../interfaces/requests/UserRequest';
import { Observable } from 'rxjs';
import {Product} from "../interfaces/responses/Product";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly apiEndpoint = environment.apiUrl + '/users';

  constructor(private http: HttpClient) {}

  getAll(page: number, size: number): Observable<HttpResponse<User[]>> {
    let params = new HttpParams()
      .set('page', page)
      .set('size', size);

    return this.http.get<User[]>(this.apiEndpoint, { params, observe: 'response' });
  }

  get(id: string): Observable<User> {
    return this.http.get<User>(this.apiEndpoint + '/' + id);
  }

  create(user: UserRequest): Observable<User> {
    return this.http.post<User>(this.apiEndpoint, user);
  }

  update(user: UserRequest): Observable<User> {
    return this.http.put<User>(this.apiEndpoint + '/' + user.id, user);
  }

  delete(id: string): Observable<User> {
    return this.http.delete<User>(this.apiEndpoint + '/' + id);
  }
}
