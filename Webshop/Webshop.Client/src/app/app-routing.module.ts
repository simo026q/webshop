import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from "./core/authGuard";
import { AdminGuard } from "./core/adminGuard";
import { PageNotFoundComponent } from "./components/page-not-found.component";

const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./components/frontpage/frontpage.component').then(m => m.FrontpageComponent),
  },
  {
    path: 'basket',
    loadComponent: () => import('./components/basket/basket.component').then(m => m.BasketComponent),
  },
  {
    path: 'login',
    loadComponent: () => import('./components/login/login.component').then(m => m.LoginComponent),
  },
  {
    path: 'register',
    loadComponent: () => import('./components/register/register.component').then(m => m.RegisterComponent),
  },
  {
    path: 'products',
    loadComponent: () => import('./components/products/products.component').then(m => m.ProductsComponent),
  },
  {
    path: 'products/product/:id',
    loadComponent: () => import('./components/products/product/product.component').then(m => m.ProductComponent),
  },
  {
    path: 'account',
    loadComponent: () => import('./components/account/account.component').then(m => m.AccountComponent),
    canActivate: [AuthGuard],
  },
  {
    path: 'account/order/:id',
    loadComponent: () => import('./components/account/order/order.component').then(m => m.OrderComponent),
    canActivate: [AuthGuard],
  },
  {
    path: 'admin',
    loadComponent: () => import('./components/admin/admin.component').then(m => m.AdminComponent),
    canActivate: [AdminGuard],
    children: [
      {
        path: 'users',
        loadComponent: () => import('./components/admin/adminusers/adminusers.component')
          .then(m => m.AdminusersComponent)
      },
      {
        path: 'users/user/:action/:id',
        loadComponent: () => import('./components/admin/adminusers/adminuser/adminuser.component')
          .then(m => m.AdminuserComponent)
      },
      {
        path: 'categories',
        loadComponent: () => import('./components/admin/admincategories/admin-categories.component')
          .then(m => m.AdminCategoriesComponent)
      },
      {
        path: 'products',
        loadComponent: () => import('./components/admin/adminproducts/adminproducts.component')
          .then(m => m.AdminproductsComponent)
      },
      {
        path: 'products/product/:action',
        loadComponent: () => import('./components/admin/adminproducts/adminproduct/adminproduct.component')
          .then(m => m.AdminproductComponent)
      },
      {
        path: 'products/product/:action/:id',
        loadComponent: () => import('./components/admin/adminproducts/adminproduct/adminproduct.component')
          .then(m => m.AdminproductComponent)
      },
      {
        path: 'orders',
        loadComponent: () => import('./components/admin/adminorders/adminorders.component')
          .then(m => m.AdminordersComponent)
      },
      {
        path: 'orders/order/:id',
        loadComponent: () => import('./components/account/order/order.component')
          .then(m => m.OrderComponent)
      }
    ]
  },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
