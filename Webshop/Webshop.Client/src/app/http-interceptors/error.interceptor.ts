import {Injectable} from "@angular/core";
import {HttpEvent, HttpHandler, HttpInterceptor,HttpRequest} from '@angular/common/http';
import {Observable, throwError} from "rxjs";
import {catchError} from 'rxjs/operators';
import {Router} from "@angular/router";
import {AuthenticationService} from "../core/services/authentication.service";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(public router: Router, private authenticationService: AuthenticationService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return next.handle(req).pipe(
      catchError((error) => {
        if (error.status === 401 || error.status === 403) {
          this.authenticationService.logout();
          return throwError(error.message);
        }
        console.log('error has been intercepted')
        window.alert(error.message);
        return throwError(error.message);
      })
    )
  }
}

