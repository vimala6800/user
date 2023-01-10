import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ResponseCode } from '../../../enums/responseCode';
import { AddUser } from '../../../models/UserAdminstration/adduser.model';
import { ResponseModel } from '../../../models/UserAdminstration/responseModel';
import { Role } from '../../../models/UserAdminstration/role';
import { User } from '../../../models/UserAdminstration/user';
import { UserService } from '../../../services/user.service';
@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  public roles: Role[] = [];
  public userList: User[] = [];
  public registerForm;
  //getAllRoles: any;
  constructor(private userService: UserService, private toastr: ToastrService, private formBuilder: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      userName: ['', [Validators.required]],
      email: ['', [Validators.email, Validators.required]],
      phoneNumber: ['', Validators.required]
    });
    this.getAllRoles();
  }
  onSubmit() {

    let userName = this.registerForm.controls["userName"].value;
    let email = this.registerForm.controls["email"].value;
    let phoneNumber = this.registerForm.controls["phoneNumber"].value;
    this.userService.register(userName, email, phoneNumber, this.roles.filter(x => x.isSelected).map(x => x.role)).subscribe((data: ResponseModel) => {
      if (data.responseCode == ResponseCode.OK) {
        this.registerForm.controls["userName"].setValue("");
        this.registerForm.controls["email"].setValue("");
        this.registerForm.controls["phoneNumber"].setValue("");
        this.roles.forEach(x => x.isSelected = false);
        this.toastr.success("You have created account please login");
        this.router.navigate(["login"]);

      } else {
        this.toastr.error(data.dateSet[0]);
      }
      console.log("response", data);
    }, error => {
      console.log("error", error)
      this.toastr.error("Something went wrong please try again later");
    })
  }
  getAllRoles() {
    this.userService.getAllRole().subscribe(roles => {
      this.roles = roles;
    });
  }
  onRoleChange(role: string) {
    this.roles.forEach(x => {
      if (x.role == role) {
        x.isSelected = !x.isSelected;
      }

    })
  }

  get isRoleSelected() {
    return this.roles.filter(x => x.isSelected).length > 0;
  }

}
 


  
