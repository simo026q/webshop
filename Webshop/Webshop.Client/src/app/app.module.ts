import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { NavbarComponent } from "./components/navbar/navbar.component";
import { AuthInterceptor } from "./http-interceptors/auth.interceptor";
import { LoggingInterceptor } from "./http-interceptors/logging.interceptor";
import { FooterComponent } from './components/footer/footer.component';
import { PageNotFoundComponent } from "./components/page-not-found.component";
import { CommonModule, NgForOf } from '@angular/common';
import { NavbarUserComponent } from './components/navbar-user/navbar-user.component';
import { NavbarBasketComponent } from './components/navbar-basket/navbar-basket.component';
import { BasketService } from "./services/basket.service";
import { ErrorInterceptor } from "./http-interceptors/error.interceptor";
import {BasketCardComponent} from "./components/basket/basket-card/basket-card.component";

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    NavbarUserComponent,
    FooterComponent,
    PageNotFoundComponent,
    NavbarBasketComponent,
  ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        CommonModule,
        NgForOf,
        BasketCardComponent
    ],
  providers: [
    BasketService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoggingInterceptor, multi: true},
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
