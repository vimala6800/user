import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { ResponseCode } from '../enums/responseCode';
import { Constants } from '../Helper/constants';
import { ForgotPasswordDto } from '../models/forgotPasswordDto.model';
import { ResetPasswordDto } from '../models/resetPasswordDto.model';
import { AddUser } from '../models/UserAdminstration/adduser.model';
import { ResponseModel } from '../models/UserAdminstration/responseModel';
import { Role } from '../models/UserAdminstration/role';
import { User } from '../models/UserAdminstration/user';
@Injectable({
  providedIn: 'root'
})
export class UserService {
  base_url: string = environment.base_url;

  constructor(private http: HttpClient) { }
  //-----------------------------------------------------------------------------------------------------------------
   //User register start
  //To register User with Roles Incoperated with it//
  public register(userName: string, email: string, phoneNumber: string, roles: string[]) {

    const body = {
      UserName: userName,
      Email: email,
      PhoneNumber: phoneNumber,
      Roles: roles,
     /* //ClientURL:'https://localhost:4200/resetpassword'*/
    }
    return this.http.post<ResponseModel>(Constants.BASE_URL + "/api/A/RegisterUser", body);
  }
  //UserList End
  //-----------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------
   //UserList start
  //To list all User in Grid with Roles Incoperated with it//
 
  public getAllUser() {
    let userInfo = JSON.parse(localStorage.getItem(Constants.USER_KEY));
    return this.http.get<ResponseModel>(Constants.BASE_URL + "/api/UserRegistration/GetAllUser").pipe(map(res => {
      let userList = new Array<User>();
      if (res.responseCode == ResponseCode.OK) {
        if (res.dateSet) {
          res.dateSet.map((x: User) => {
            userList.push(new User(x.id, x.userName, x.email, x.phoneNumber, x.roles));
          })
        }
      }
      return userList;
    }));
  }
  //UserList End
  //-----------------------------------------------------------------------------------------------------------------
  public getAllRole() {
    return this.http.get<ResponseModel>(Constants.BASE_URL + "/api/UserRegistration/GetRoles", ).pipe(map(res => {
      let roleList = new Array<Role>();
      if (res.responseCode == ResponseCode.OK) {
        if (res.dateSet) {
          res.dateSet.map((x: string) => {
            roleList.push(new Role(x));
          })
        }
      }
      return roleList;
    }));
  }
  
  }
  

