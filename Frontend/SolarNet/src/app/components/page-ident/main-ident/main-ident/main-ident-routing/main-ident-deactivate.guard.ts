import { Injectable } from '@angular/core';
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { NewAdvertismentComponent } from '../../../new-advertisment/new-advertisment.component';

@Injectable({
  providedIn: 'root'
})
export class MainIdentDeactivateGuard implements CanDeactivate<NewAdvertismentComponent> {
  canDeactivate(
    component: NewAdvertismentComponent,
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    
    return !component.getFormStatus() || component.canRedirect || confirm("Введенные данные не будут сохранены. Продолжить?");
  }
}