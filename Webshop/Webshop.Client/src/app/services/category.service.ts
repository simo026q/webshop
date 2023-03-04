import { environment } from '../core/environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Category } from '../interfaces/responses/Category';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private readonly apiEndpoint = environment.apiUrl + '/categories';

  constructor(private http: HttpClient) {}

  getAllWithProducts(): Observable<Category[]> {
    return this.http.get<Category[]>(this.apiEndpoint);
  }

  getAll(): Observable<Category[]> {
    return this.http.get<Category[]>(this.apiEndpoint + '/all');
  }

  create(category: Category): Observable<Category> {
    return this.http.post<Category>(this.apiEndpoint, category);
  }

  delete(id: string): Observable<Category> {
    return this.http.delete<Category>(this.apiEndpoint + '/' + id);
  }
}
