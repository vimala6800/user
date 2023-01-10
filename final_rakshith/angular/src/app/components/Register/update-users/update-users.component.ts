import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Register } from '../../../models/register.model';
import { RegisterService } from '../../../services/register.service';

@Component({
  selector: 'app-update-users',
  templateUrl: './update-users.component.html',
  styleUrls: ['./update-users.component.scss']
})
export class UpdateUsersComponent implements OnInit {
  myform!: FormGroup;
  getUserRequest: Register = {
      id: '',
      userName: '',
      email: '',
      phoneNumber: ''
      
  }
  constructor(private router: Router, private registerService: RegisterService, private toastr: ToastrService, private route: ActivatedRoute, private formBuilder: FormBuilder) { }
  ngOnInit(): void {
    this.myform = new FormGroup({
      "userName": new FormControl(null, Validators.required),
      "email": new FormControl(null, [Validators.email, Validators.required]),
      "phoneNumber": new FormControl(null, Validators.required),
    })
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');

        if (id) {
          this.registerService.getUser(id).subscribe({
            next: (response) => {
              this.getUserRequest = response;
            }
          })

        }
      }
    })

  }
  updateuser() {
    this.registerService.updateUser(this.getUserRequest.id, this.getUserRequest)
      .subscribe({
        next: (response) => {
          this.toastr.success("Update User-Detail Successfully");
          this.router.navigate(['UserAdminstration'])
        }
      });
  }
  deleteuser(id: String) {
    this.registerService.deleteUser(id).subscribe({
      next: (response) => {
        this.toastr.success("Deleted User-Detail Successfully");
        this.router.navigate(['/UserAdminstration'])
      }
    });
  }
}


