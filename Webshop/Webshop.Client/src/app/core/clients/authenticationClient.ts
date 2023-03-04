import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from "../../interfaces/responses/User";
import { AuthResponse } from "../../interfaces/responses/AuthResponse";
import { AuthRequest } from "../../interfaces/requests/AuthRequest";

@Injectable({
  providedIn: 'root',
})
export class AuthenticationClient {
  constructor(private http: HttpClient) {}

  public login(
    authRequest: AuthRequest,
  ): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      environment.apiUrl + '/Auth/authenticate',
      {
        email: authRequest.email,
        password: authRequest.password,
      },
    );

  }

  public register(
    fullName: string,
    email: string,
    password: string
  ): Observable<User> {
    return this.http.post<User>(
      environment.apiUrl + '/Account/register',
      {
        email: email,
        password: password,
        fullName: fullName,
      },
    );
  }
}
