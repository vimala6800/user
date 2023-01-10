import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ResponseCode } from '../../../enums/responseCode';
import { UserService } from '../../../services/user.service';
import { Role } from '../../../models/UserAdminstration/role';
import { User } from '../../../models/UserAdminstration/user';
import { ResponseModel } from '../../../models/UserAdminstration/responseModel';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginService } from '../../../services/login.service';
import { ForgotPasswordDto } from '../../../models/forgotPasswordDto.model';
import { HttpErrorResponse } from '@angular/common/http';
import { Register } from '../../../models/register.model';

@Component({
  selector: 'app-user-adminstration',
  templateUrl: './user-adminstration.component.html',
  styleUrls: ['./user-adminstration.component.scss']
})
export class UserAdminstrationComponent implements OnInit {
  myform!: FormGroup;
  successMessage: string;
  errorMessage: string;
  showSuccess: boolean;
  showError: boolean;
  public roles: Role[] = [];
  totalCount: any;
  public userList: User[] = [];
  public registerForm;
  searchText: any;
  totalLength: any;
  noOfRows = 10;
  page: number = 1;
  //getAllRoles: any;
  
  constructor(private userService: UserService,private route:ActivatedRoute, private toastr: ToastrService, private loginService: LoginService, private formBuilder: FormBuilder, private router: Router ) { }

  ngOnInit(): void {

    /*this.myform = new FormGroup({
      "userName": new FormControl(null, Validators.required),
      "email": new FormControl(null, Validators.required),
      "phoneNumber": new FormControl(null, Validators.required),
      
    })*/
    this.getAllUser();

    this.myform = this.formBuilder.group({
      userName: ['', [Validators.required]],
      email: ['', [Validators.email, Validators.required]],
      phoneNumber: ['', Validators.required]
    });
    this.getAllRoles();
    
  }
  getAllUser() {

    this.userService.getAllUser().subscribe((data: User[]) => {
      this.userList = data;
      this.totalCount = this.userList.length;
    }, () => {
    })
  }
  onSubmit() {

    let userName = this.myform.controls["userName"].value;
    let email = this.myform.controls["email"].value;
    let phoneNumber = this.myform.controls["phoneNumber"].value;
    let ClientURI = 'https://localhost:4200/resetpassword';
    this.userService.register(userName, email, phoneNumber ,this.roles.filter(x => x.isSelected).map(x => x.role)).subscribe((data: ResponseModel) => {
      if (data.responseCode == ResponseCode.OK) {
        this.myform.controls["userName"].setValue("");
        this.myform.controls["email"].setValue("");
        this.myform.controls["phoneNumber"].setValue("");
        this.roles.forEach(x => x.isSelected = false);
        this.success();
      } else {
        this.toastr.error(data.dateSet[0]);
      }
      console.log("response", data);
    }, error => {
      console.log("error", error)
      this.toastr.error("Something went wrong please try again later");
      this.router.navigate(["UserAdminstration"]);
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
  public success() {
    this.toastr.success("You have created account please login");
    this.router.navigate(['/..'], {relativeTo:this.route});
  }

  get isRoleSelected() {
    return this.roles.filter(x => x.isSelected).length > 0;
  }
  public forgotPassword = (myform) => {
    this.showError = this.showSuccess = false;
    const forgotPass = { ...myform };
    const forgotPassDto: ForgotPasswordDto = {
      email: forgotPass.email,
      clientURI: 'https://localhost:4200/resetpassword'
    }

    this.loginService.forgotPassword(forgotPassDto)
      .subscribe({
        next: (_) => {
          this.showSuccess = true;
          this.successMessage = 'The link has been sent.'
        },
        error: (err: HttpErrorResponse) => {
          this.showError = true;
          this.errorMessage = "User not found. Contact admin.";
        }
      })
  }
}
