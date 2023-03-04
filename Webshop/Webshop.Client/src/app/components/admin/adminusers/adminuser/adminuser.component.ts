import {Component, OnInit} from '@angular/core';
import {UserService} from "../../../../services/user.service";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {User} from "../../../../interfaces/responses/User";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-adminuser',
  templateUrl: './adminuser.component.html',
  styleUrls: ['./adminuser.component.scss'],
  imports: [
    FormsModule,
    RouterLink
  ],
  standalone: true
})
export class AdminuserComponent implements OnInit {

  user: User = {
    id: '',
    email: '',
    addressId: '',
    fullName: '',
    role: 0,
    isActive: true,
    createdAt: new Date().toString(),
  }

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
      this.route.paramMap.subscribe(params => {
          const id = params.get('id')!;
          this.userService.get(id).subscribe(user => {
            this.user = user;
          });
      });
  }

  saveUser() {
    this.userService.update(this.user).subscribe(() => {
      this.router.navigate(['/admin/users']);
    })
  }

}
