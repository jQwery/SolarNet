import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AppService } from 'src/app/app.service';

@Injectable({
  providedIn: 'root'
})
export class MainIdentGuard implements CanActivate {

  constructor(private appService: AppService, private router: Router){//ссылка на главный сервис для получения статуса авторизации, 
                                                                      // и ссылка на route для перенаправления на стартовую страницу 
  }
  

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if(this.appService.getLoggedStatus() != "user"){//если доступа нет
      this.router.navigate(['']);//перенаправление на стартовую страницу
      return true;
    }
    return true//статус авторизации, переход разрешен если есть доступ, и запрещен если доступа нет
  }
}
