import { Component, ComponentFactoryResolver, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AppService } from 'src/app/app.service';
import { ACCES_TOKEN_KEY, AuthorizationServiceService } from '../../authorization/authorization-module/authorization-service.service';
import { RegistrationComponent } from '../../authorization/registration/registration.component';
import { SignInComponent } from '../../authorization/sign-in/sign-in.component';
import { MainIdentServiceService } from '../main-ident/main-ident-service.service';
import { PageIdentService } from '../page-ident.service';

@Component({
  selector: 'app-header-ident',
  templateUrl: './header-ident.component.html',
  styleUrls: ['./header-ident.component.css']
})
export class HeaderIdentComponent implements OnInit {

  @ViewChild('authorization', {read: ViewContainerRef}) authorization:any;

  avatarLink: string = '';

  searchParam: any = {//обьект с параметрами обьявления
    idCategory: '',
    keyWords: ''
  }
    
  constructor(private mServ: MainIdentServiceService, 
              private appServ: AppService, 
              private pIdentServ: PageIdentService, 
              private componentFactoryResolver: ComponentFactoryResolver, 
              private authServ: AuthorizationServiceService,
              public service: AppService,
              private jwtHelper: JwtHelperService,
              private router: Router) { 
    this.closeAuthorizationBars();//Добваление слушателя событий по клику на элемент
    this.authServ.getIsLoggedInfo().subscribe(data => {//Возвращает информацию о статусе авторизации пользователя
      if(data){this.clearAuth();}//Все окна регистрации и входа закрываются
    })
    this.pIdentServ.getUserValuesInfo().subscribe(data => {//Получение информации об аватарке пользователя
      this.avatarLink = data.avatarLink;
    })
    this.mServ.getParamsInfo().subscribe(params =>{//При изменении параметров поиска обьявлений, в шапке устанавливаются соответсвенные параметры
      this.setParamsFromService(params);
    })
    this.hideCategoryList();//Добавление слушателя событий при наведении на элемент навигационного меню
  }

  getLoggedStatus(): string{//для правильной отрисовки элементов в зависимости от роли 
    return this.appServ.getLoggedStatus();
  }

  showAuthorizationBars(ev:Event): void{
    this.authorization.clear();//после нажатия на кнопу авторизации предыдущее представление стирается
    var tg = ev.target as HTMLTextAreaElement;//явно указан тип, чтобы язык понимал с чем работает
    if (tg.className == 'registration-btn'){//кнопка регистрации
      let regItemComponent = this.componentFactoryResolver.resolveComponentFactory(RegistrationComponent);//создание компонента
      let regItemComponentRef = this.authorization.createComponent(regItemComponent);
      this.closeAuthorizationBars();//вызов функции-обработчика событий по нажатию на кнопку-закрыть форму
    }else{//кнопка входа
      let signInItemComponent = this.componentFactoryResolver.resolveComponentFactory(SignInComponent);
      let signInItemComponentRef = this.authorization.createComponent(signInItemComponent);
      this.closeAuthorizationBars();
      this.openRegBar();//из формы входа есть возможность перейти в форму регистрации, при этом форма входа закрывется
    }
  }

  closeAuthorizationBars(): void{//обработчик события закрытия формы
    let closeBtn = document.querySelector('.close-btn');//кнопка крестик
    closeBtn?.addEventListener('click',  ()=>{//при нажатии на крестик
      this.authorization.clear();//представление стирается
    })
  }

  clearAuth(): void{//Закрыть модальные окна регистрации и входа
    this.authorization.clear();
  }

  openRegBar(): void{//переход к регистрации из входа
    document.querySelector('.reg-btn')?.addEventListener('click', ()=>{//кнопка регистрации, клик по ней
      this.authorization.clear();//удаление формы входа
      let regItemComponent = this.componentFactoryResolver.resolveComponentFactory(RegistrationComponent);
      let regItemComponentRef = this.authorization.createComponent(regItemComponent);
      this.closeAuthorizationBars();//так же вызов функции обработчика, чтобы из данного сценария добавить обработчик на кнопку-закрыть
    })
  }

  showCategoryList(ev: Event): void{//для адаптива список категорий скрывается
    document.querySelector('.nav-menu')?.classList.add('nav-menu-shown');
    // console.log('open')
  }

  hideCategoryList(): void{//При смещении курсора с элемента, выпадающее меню закрывается
    document.addEventListener('mouseover', function(ev){
      let menu = document.querySelector('.nav-menu') as HTMLTextAreaElement;
      menu.classList.remove('nav-menu-shown');
    })
    
  }

  showHideUserRoom(): void{//Для маленьких экранов некоторые кнопки обьеденены в одном месте, это выпадающее меню для 
    document.addEventListener('mousedown', function(ev){//моих обьявлений и личного кабинета
      let tg = ev.target as HTMLTextAreaElement;
      let userRoom = document.querySelector('.user-room') as HTMLTextAreaElement;
      let userProfile = document.querySelector('.user-profile-mini') as HTMLTextAreaElement;
      if(userProfile.contains(tg)){
        userRoom.classList.toggle('none');
      }else{
        userRoom.classList.add('none');
      }
    })

  }

  ngOnInit(): void {
    var tokenDecode: any[];//автоматический вход
    if(localStorage.getItem(ACCES_TOKEN_KEY) != null){//Сразу попытка считать токен авторизации
      tokenDecode = Object.values(this.jwtHelper.decodeToken(localStorage.getItem(ACCES_TOKEN_KEY)?.toString()));//При успехе, он декодируется и 
      this.changeLoggedStatus(tokenDecode);//вид шапки меняется в зависимости от роли пользователя
    }
  }

  changeLoggedStatus(tokenDecode: any[]): void{//для смены статуса авторизации
    this.service.setLoggedStatus(tokenDecode[5].toLowerCase());
    this.avatarLink = tokenDecode[4];
  }

  changeParamsForm(): void{//смена параметров с сервиса
    this.mServ.setAllCategoryParamsSearch(this.searchParam);    
    this.mServ.setAdvsType('params');
  }

  addValueToInput(ev:Event): void{//функция добавляет содержимое выбранного пункта в строку input, после чего убирает выпадающее меню
    let tg = ev.target as HTMLTextAreaElement;
    if(tg.classList.contains('search') || tg.classList.contains('search-btn')){
      this.changeParamsForm();
    }
    
  }

  addUserAdvList(): void{//смена типа выводимых обьявлений
    this.mServ.setAdvsType('user');//для пользовательских обьявлений
  }

  addNotApprovedAdvList(): void{//для запросов на обьявления
    this.mServ.setAdvsType('notApproved');
  }

  addAllUsersList(): void{//для вывода списка пользователей
    this.mServ.setAdvsType('usersData');
  }

  clearAllParams(): void{//очистка параметров    
    this.mServ.clearAllCategoryParams();    
  }

  setParamsFromService(params:any): void{//Берутся значения с сервиса, где храница обьект значений параметров
    this.searchParam.idCategory = params.idCategory;
    this.searchParam.keyWords = params.keyWords;
  }

  getCategoryId(ev: Event): void{//При клике на категорию в меню навигации, сразу же изменяются параметры поиска
    let tg = ev.target as HTMLTextAreaElement;
    console.log(tg.id);
    this.searchParam.idCategory = tg.id;
    // this.router.navigate(['']);
    this.mServ.setAllCategoryParamsSearch(this.searchParam);//Изменение параметров
    this.mServ.setAdvsType('params');//Изменение типа обьявлений
  }

  exit(): void{//Выход из учетной записи
    this.authServ.logout();
    this.authServ.setIsLoggedInfo(false);
  }

}
