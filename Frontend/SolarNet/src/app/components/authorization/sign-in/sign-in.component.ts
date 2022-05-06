import { Component, ComponentFactoryResolver, EventEmitter, OnInit, Output, ViewChild, ViewContainerRef } from '@angular/core';
import { AppService } from 'src/app/app.service';
import { ACCES_TOKEN_KEY, AuthorizationServiceService } from '../authorization-module/authorization-service.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {
  @ViewChild('Form', {read: ViewContainerRef}) form: any;

  constructor(private aServ: AuthorizationServiceService,
              private componentFactoryResolver: ComponentFactoryResolver, 
              public service: AppService,
              private jwtHelper: JwtHelperService) { }

  tokenDecode: any[] = [];//Для полей расшифрованного токена

  login(email: string, password: string) {//Вход в приложение
    this.aServ.login(email, password).subscribe(res =>{
      this.tokenDecode = Object.values(this.jwtHelper.decodeToken(res.access_token));//значения обьекта расшифрованного токена
      this.changeLoggedStatus();
      this.aServ.setIsLoggedInfo(true);//смена статуса авторизации
    }, error =>{
      if(error.status == 200){//На ранних этапах приложение выдавало ошибку 200 при успешном входе
        console.log(localStorage.getItem(ACCES_TOKEN_KEY));
      }else{
        console.log(error.status);//Если поля неверные, пользователь получит сообщение об ошибке
        alert("Вы ввели неверные данные")
      }
    })
  }

  getSignInValues(): any{//Значения полей авторизации
    return this.aServ.signInFormValues;
  }

  printForm(): void{
    console.log(this.aServ.signInFormValues);
  }

  changeLoggedStatus(): void{//для смены статуса авторизации
    this.service.setLoggedStatus(this.tokenDecode[5].toLowerCase());
    // console.log(this.tokenDecode[5].toLowerCase());
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void{
    this.aServ.clearSignInFormValues();//функция очистки обьекта, где хранятся данные формы в сервисе
  }

}
