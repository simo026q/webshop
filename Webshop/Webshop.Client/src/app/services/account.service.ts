import { environment } from '../core/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from "../interfaces/responses/User";
import { UserRequest } from "../interfaces/requests/UserRequest";

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private readonly apiEndpoint = environment.apiUrl + '/account';

  constructor(private http: HttpClient) {}

  getAccount(): Observable<User> {
    return this.http.get<User>(this.apiEndpoint);
  }

  updateAccount(userRequest: UserRequest): Observable<User> {
    return this.http.put<User>(this.apiEndpoint, userRequest);
  }

  deleteAccount(): Observable<User> {
    return this.http.delete<User>(this.apiEndpoint);
  }
}
