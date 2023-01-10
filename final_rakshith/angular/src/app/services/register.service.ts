import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from 'oidc-client';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Register } from '../models/register.model';
@Injectable({
  providedIn: 'root'
})
export class RegisterService {
  base_url: string = environment.base_url;
  constructor(private http: HttpClient) { }
  updateUser(id: String, updateUserRequest: Register): Observable<Register> {
    return this.http.put<Register>(this.base_url + '/api/Users/' + id, updateUserRequest)
  }
  getUser(id: string): Observable<Register> {
    return this.http.get<Register>(this.base_url + '/api/Users/' + id);
  }
  deleteUser(id: String): Observable<Register> {
    return this.http.delete<Register>(this.base_url + '/api/Users/' + id);
  }
}
