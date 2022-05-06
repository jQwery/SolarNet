import { Injectable } from '@angular/core';
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { UserPageComponent } from '../../../user-page/user-page.component';

@Injectable({
  providedIn: 'root'
})
export class MainIdentDeactivateUserPageGuard implements CanDeactivate<UserPageComponent> {
  canDeactivate(
    component: UserPageComponent,
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

      return !component.getFormStatus() || component.canRedirect || confirm("Введенные данные не будут сохранены. Продолжить?");
    }
  
}
