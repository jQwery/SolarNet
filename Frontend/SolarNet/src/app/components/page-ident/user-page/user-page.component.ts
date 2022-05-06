import { ThrowStmt } from '@angular/compiler';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AppService } from 'src/app/app.service';
import { ACCES_TOKEN_KEY, AuthorizationServiceService } from '../../authorization/authorization-module/authorization-service.service';
import { PageIdentService } from '../page-ident.service';

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html',
  styleUrls: ['./user-page.component.css']
})
export class UserPageComponent implements OnInit {

  @ViewChild('userForm') userForm:any;

  tokenDecode: any[] = [];

  buttonType: any = '';

  canRedirect: boolean = false;//При отправке данных на сервер, менять значение!!!!!!!!!!!!!!!!!!!!!!!!!!!1

  getUserValues(): any{//получает из сервиса обьект данных пользователя
    return this.pIdentServ.userValues;
  }

  printUserValues(): void{
    console.log(this.pIdentServ.userValues);
  }

  showAvatarChangeBar(ev:Event): void{// Показывает форму для смены фото
    let avChangeBar = document.querySelector('.change-av-img-container') as HTMLTextAreaElement;
    avChangeBar.classList.remove('none');
  }

  changeAvatar(ev:Event): void{ // Изменяет значение сслыки на фото в обьекте userValues
    let avValue = document.querySelector('input.adv-img-link-input') as HTMLTextAreaElement;
    this.pIdentServ.userValues.avatarLink = avValue.value;
    let avValueParent = avValue.parentNode as HTMLTextAreaElement;
    avValueParent.classList.add('none');//и скрывает его
  }

  closeAvatarChangeBar(ev:Event): void{//скрывает форму смены фото
    let avChangeBar = document.querySelector('.change-av-img-container') as HTMLTextAreaElement;
    avChangeBar.classList.add('none');
  }

  getFormStatus(): boolean{
    return this.userForm.dirty;
  }

  constructor(private pIdentServ: PageIdentService, 
              private authServ: AuthorizationServiceService, 
              private service: AppService, 
              private jwtHelper: JwtHelperService,
              private router: Router) { }

  ngOnInit(): void {
    this.pIdentServ.decodeToken();//Для заполнения данных пользователя
  }

  changeUserValues(condition: any): void{//Изменение данных пользователя
    if(condition == "1"){//Изменение данных
      this.pIdentServ.changeUserValues(this.pIdentServ.getUserValues()).subscribe( ()=> {
        this.loginAgain();//При изменении данных, происходит повторный вход для моментаьного отображения изменений
      }, (error) => {
        if(error.status == 500){
          alert("Введите правильный пароль");
        }
      })
    }else{
      this.changeUserAvatar();//Изменение аватарки
    }

  }

  changeUserAvatar(): void{//Изменение аватра пользователя
    this.pIdentServ.changeUserAvatar(this.pIdentServ.getUserValues()).subscribe((res) => {
      console.log(res);
      this.loginAgain();
    }, (error) => {
        switch(error.status){//описание всех ошибок, которые могут быть вызваны
          case 200:
            console.log(localStorage.getItem(ACCES_TOKEN_KEY));
            break;
          case 400:
            alert("Для внесения изменений в личные данные подтвердите пароль");
            break;
          case 500:
            alert("Введите правильный пароль");
            break;
        }
      })
  }

  loginAgain(): void{//Повторный вход
    if(this.pIdentServ.getUserValues().passwordAgain != ''){//если менялся пароль, то вход происходит по новому
      this.authServ.login(this.pIdentServ.getUserValues().mail, this.pIdentServ.getUserValues().passwordAgain).subscribe(res => {
        this.tokenDecode = Object.values(this.jwtHelper.decodeToken(res.access_token));//значения обьекта расшифрованного токена
        this.changeLoggedStatus();
        this.pIdentServ.clearUserValues();
        this.pIdentServ.decodeToken();
        this.router.navigate(['']);
        this.canRedirect = true;
      }, error =>{
        })
    }else{
      this.authServ.login(this.pIdentServ.getUserValues().mail, this.pIdentServ.getUserValues().password).subscribe(res => {//иначе по старому паролю
        this.tokenDecode = Object.values(this.jwtHelper.decodeToken(res.access_token));//значения обьекта расшифрованного токена
        this.changeLoggedStatus();
        this.pIdentServ.decodeToken();
        this.router.navigate(['']);
        this.canRedirect = true;
      }, error =>{
          switch(error.status){
            case 200:
              console.log(localStorage.getItem(ACCES_TOKEN_KEY));
              break;
            case 400:
              alert("Для внесения изменений в личные данные подтвердите пароль");
              break;
            case 500:
              alert("Введите правильный пароль");
              break;
          }
        })
      } 
    }

  changeLoggedStatus(): void{//для смены статуса авторизации
    this.service.setLoggedStatus(this.tokenDecode[5].toLowerCase());
  }

  exit(): void{
    this.authServ.logout();
    this.authServ.setIsLoggedInfo(false);
  }

  selectedFile: any;

  fileName = '';

  onFileSelected(event: any) {//Загрузка аватарки пользователя

    const file:File = event.target?.files[0];

    if (file) {
        this.fileName = file.name;
        const formData = new FormData();
        formData.append('Name', file.name);
        formData.append('Image', file);
        this.pIdentServ.uploadFile(formData).subscribe(data => {
          console.log(data);
          this.pIdentServ.userValues.avatarLink = data;
        });
    }
  }


  openPasswordChangeModule(ev:Event): void{//Модальное окно со сменой пароля
    let passWordChangeModule = document.querySelector('.new-password-container') as HTMLTextAreaElement;
    passWordChangeModule.classList.remove('none');
  }

  openPasswordConfirmModule(ev:Event): void{//Окно с подтверждением текущего пароля
    let tg = ev.target as HTMLTextAreaElement;
    if (tg.classList.contains('change-avatar')){
      this.buttonType = "avatar";
      document.querySelector('.passwords-container')?.classList.remove('none');
    }else{
      if(this.pIdentServ.getUserValues().passwordAgain != this.pIdentServ.getUserValues().passwordAgainSub){
        alert("Пароли не совпадают");
      }else{
        this.buttonType = "data";
        document.querySelector('.passwords-container')?.classList.remove('none');
      }
      
    }
  }

  closePasswordsModules(ev:Event): void{//При нажатии на крестик, окна закрываются
    document.querySelector('.new-password-container')?.classList.add('none');
    document.querySelector('.passwords-container')?.classList.add('none');
    this.pIdentServ.userValues.passwordAgainSub = '';
    this.pIdentServ.userValues.passwordAgain = '';
    this.pIdentServ.userValues.password = '';
  }

}
