import { Component, ComponentFactoryResolver, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
// import { stat } from 'node:fs';
import { AppService } from 'src/app/app.service';
import { AdvertismentAdminComponent } from 'src/app/components/page-admin/main-admin/advertisments-table-admin/advertisment-admin/advertisment-admin.component';
import { MainIdentServiceService} from '../main-ident-service.service';
import { UserComponent } from '../user/user.component';
import { AdvertismentIdentComponent } from './advertisment-ident/advertisment-ident.component';
import { Adv } from './models/Advertisment';

@Component({
  selector: 'app-advertisment-content-ident',
  templateUrl: './advertisment-content-ident.component.html',
  styleUrls: ['./advertisment-content-ident.component.css']
})
export class AdvertismentContentIdentComponent implements OnInit {
  @ViewChild('books', {read: ViewContainerRef}) book:any;

  // @ViewChild('users', {read: ViewContainerRef}) user:any;

  interval: any;
  // advId:number = 1;

  currentAdvListInfo: string = 'Все обьявления';

  sortForm: any = {//обьект с параметрами для обьявлений
    sortParamType: '',
    sortType: ''
  }

  showHiddenAdvsField: any = {
    showHiddenAdvs: true
  }

  paginationForm: any ={
    offset: 0
  }

  isUserAdvs: any = {//для статуса пользовательских обьявлений
    isUserAdvsStatus: false
  };

  //Переменные для правильного отображения пустого массива данных
  isAdvertisments = true;
  isUserAdvertisments = true;
  isCommonAdvertisments = true;
  isUsersList = false;
  notApprovedListEmptyNow = false;
  otherUserListEmptyNow = false;

  //Переменные для пагинации
  prevPageNumber = 0;
  currentPageNumber: any;
  nextPageNumber: any;
  lastPageNumber: any;

  // readonly Status = Status;
  // readonly Adv: Pending<Adv[]>;

  constructor(private componentFactoryResolver: ComponentFactoryResolver, private mServ: MainIdentServiceService, private service: AppService) {
    this.mServ.getParamsInfo().subscribe(() =>{//Подписка на изменение параметров поиска
        this.getAdvs();//Получение обьявлений
        this.counts = 0;
        this.currentPageNumber = this.mServ.getParams().offset/this.mServ.getParams().limit + 1;//Работа пагинации
        this.nextPageNumber = this.currentPageNumber+1;
        this.setParamsFromService();//Установка параметров из сервиса
    });//подписка на изменение параметров обьявлений
  }
  
  ngOnInit(): void {   
  }
//Последняя страница для разных типов обьявлний
  getLastPage(status: any): void{
    this.mServ.getLastPageNumber(status).subscribe(data => {
      this.lastPageNumber = data;
      console.log(data);
    })
  }

  getLastPageForMyAdvs(): void{
    this.mServ.getLastPageNumberForMyAdvs().subscribe(data => {
      this.lastPageNumber = data;
      console.log(data);
    })
  }

  getLastPageForUserList(): void{
    this.mServ.getLastPageNumberForUserList().subscribe(data => {
      this.lastPageNumber = data;
      console.log(data);
    })
  }

  getLastPageNumberForOtherUserAdvs(): void{
    this.mServ.getLastPageNumberForOtherUserAdvs(this.mServ.otherUserId).subscribe(data => {
      this.lastPageNumber = data;
      console.log(data);
    })
  }

  counts = 0;

  //метод, задающий тайм аут, интервал нулевой, но он позволяет перешагнуть на следующий макроэтап отображения контента
  //то есть к тому моменту когда все прогрузится, если его не использовать, то переменная в ViewChild не будет обработана и вернет 
  //undefined
  getAdvs() {
    this.interval = setTimeout(() => {
      switch(this.mServ.getParams().advListType){//в зависимости от типа обьявлений будет использоваться отдельный запрос
        case 'start':{
          this.counts++;
          if(this.counts > 1){
            break;
          }
          this.currentAdvListInfo =  'Все обьявления';
          console.log('s');
          if(this.getAdminStatus()){
            this.addAdminAdvs()
          }else{
            this.addAdvs();
          }
          break;
        }
        case 'params': {
          this.counts++;
          if(this.counts > 1){
            break;
          }
          this.currentAdvListInfo =  'Все обьявления';
          console.log('p');
          // debugger;
          if(this.getAdminStatus()){
            this.addAdminAdvs()
          }else{
            this.addAdvs();
          }
          break;
        }
        case 'user':{
          this.currentAdvListInfo =  'Мои обьявления';
          this.addUserAdvs();
          break;
        }
        case 'notApproved':{
          this.currentAdvListInfo =  'Необработанные обьявления';
          this.addNotApprovedAdvs();
          break;
        }
        case 'otherUser':{
          this.currentAdvListInfo =  'Обьявления пользователя: ' + this.mServ.otherUserName;
          this.addOtherUserAdvs();
          break;
        }
        case 'usersData':{
          this.addAllUsers();
          break;
        }
      }
      if(this.currentPageNumber != 1){//тут происходит и изменение пагинации
        this.prevPageNumber = this.currentPageNumber -1;
      }else{
        this.prevPageNumber = 0;
      }
      this.nextPageNumber = this.currentPageNumber+1;
    }, 250);
  }

  addAdvs(): void{//Метод возвращает список обьявлений по определенным параметрам
    this.book.clear();//Очистка текущего состояния обьявлений
    this.getAdvertisments();
  }

  addAdminAdvs(): void{
    this.book.clear();
    this.getAdminAdvertisments();
  }  
  
  addUserAdvs(): void{
    this.book.clear();
    this.getUserAdvertisments();
  }

  addNotApprovedAdvs(): void{
    this.book.clear();
    this.getNotApprovedAdvertisments();
  }

  addOtherUserAdvs(): void{
    this.book.clear();
    this.getOtherUserAdvertisments();
  }

  addAllUsers(): void{
    this.book.clear();
    this.getAllUsers();
  }

  getAdvertisments(): void{//Вывод обьявлений с параметрами на экран
    this.mServ.getAdvertismentsFromServer().subscribe(data => {
      console.log(data);
      if(data.length == 0){
        this.isAdvertisments = false;//изменение переменных для корректного отбражения информации об обьявлениях
        this.isCommonAdvertisments = false;
        this.isUserAdvertisments = true;
      }else{
        this.isAdvertisments = true;
        this.isUserAdvs.isUserAdvsStatus = false;
        this.getLastPage('1');
        for( let i = 0; i < data.length; i++){
            let bookItemComponent = this.componentFactoryResolver.resolveComponentFactory(AdvertismentIdentComponent)// берется шаблон компонента
            let bookItemComponentRef = this.book.createComponent(bookItemComponent);// по шаблону создается элемент
            (<AdvertismentIdentComponent>(bookItemComponentRef.instance)).value = data[i]//данные берутся из сервиса
        }
      }
    })
  }

  getUserAdvertisments(): void{//вывод пользовательских обьявлений
    this.mServ.getUserAdvList().subscribe(data => {
      console.log(data);
      // this.isUsersList = false;
      if(data.length == 0){
        this.isAdvertisments = false;
        this.isUserAdvertisments = false;
        this,this.isCommonAdvertisments = true;
      }else{
        this.isAdvertisments = true;
        this.isUserAdvertisments = true;
        this.isUserAdvs.isUserAdvsStatus = true;
        this.getLastPageForMyAdvs();
        for( let i = 0; i < data.length; i++){
            let bookItemComponent = this.componentFactoryResolver.resolveComponentFactory(AdvertismentIdentComponent)// берется шаблон компонента
            let bookItemComponentRef = this.book.createComponent(bookItemComponent);// по шаблону создается элемент
            (<AdvertismentIdentComponent>(bookItemComponentRef.instance)).value = data[i];
            (<AdvertismentIdentComponent>(bookItemComponentRef.instance)).value.canChange = true//данные берутся из сервиса
        }
      }
    }) 
  }

  getAdminAdvertisments(): void{//Создание обьявлений для админа
    this.mServ.getAdvertismentsFromServer().subscribe(data => {
      console.log(data);
      if(data.length == 0){
        this.isAdvertisments = false;
        this.isCommonAdvertisments = false;
        this.isUserAdvertisments = true;
      }else{
        this.isCommonAdvertisments = true;
        this.isAdvertisments = true;
        this.isUsersList = false;
        this.isAdvertisments = true;
        this.isUserAdvs.isUserAdvsStatus = false;
        this.getLastPage('1');
        for( let i = 0; i < data.length; i++){
            let bookItemComponent = this.componentFactoryResolver.resolveComponentFactory(AdvertismentAdminComponent)// берется шаблон компонента
            let bookItemComponentRef = this.book.createComponent(bookItemComponent);// по шаблону создается элемент
            (<AdvertismentAdminComponent>(bookItemComponentRef.instance)).value = data[i]//данные берутся из сервиса
        }
      }
    })
  }

  getNotApprovedAdvertisments(): void{//Создание обьявлений для запросов
    this.mServ.getNotApprovedAdvList().subscribe(data => {
      console.log(data);
      this.isUsersList = false;
      this.isUserAdvs.isUserAdvsStatus = false;
      if(data.length == 0){
        this.isAdvertisments = false;
        this.notApprovedListEmptyNow = true;
        this.otherUserListEmptyNow = false;
      }else{
        this.isAdvertisments = true;
        this.getLastPage('0');
        for( let i = 0; i < data.length; i++){
          let bookItemComponent = this.componentFactoryResolver.resolveComponentFactory(AdvertismentAdminComponent)// берется шаблон компонента
          let bookItemComponentRef = this.book.createComponent(bookItemComponent);// по шаблону создается элемент
          (<AdvertismentAdminComponent>(bookItemComponentRef.instance)).value = data[i]//данные берутся из сервиса
        }
      }
    })
  }

  getOtherUserAdvertisments(): void{//Вывод обьявлений другого пользователя
    this.mServ.getOtherUserAdvList(this.mServ.otherUserId).subscribe(data => {
      console.log(data);
      this.isUsersList = false;
      this.isUserAdvs.isUserAdvsStatus = false;
      if(data.length == 0){
        this.isAdvertisments = false;
        this.notApprovedListEmptyNow = false;
        this.otherUserListEmptyNow = true;
      }else{
        this.isAdvertisments = true;
        this.getLastPageNumberForOtherUserAdvs();
        if(this.getAdminStatus()){
          for( let i = 0; i < data.length; i++){
            let bookItemComponent = this.componentFactoryResolver.resolveComponentFactory(AdvertismentAdminComponent)// берется шаблон компонента
            let bookItemComponentRef = this.book.createComponent(bookItemComponent);// по шаблону создается элемент
            (<AdvertismentAdminComponent>(bookItemComponentRef.instance)).value = data[i]//данные берутся из сервиса
          }
        }else{
          for( let i = 0; i < data.length; i++){
            let bookItemComponent = this.componentFactoryResolver.resolveComponentFactory(AdvertismentIdentComponent)// берется шаблон компонента
            let bookItemComponentRef = this.book.createComponent(bookItemComponent);// по шаблону создается элемент
            (<AdvertismentIdentComponent>(bookItemComponentRef.instance)).value = data[i];
          }
        }
      }
    })
  }

  getAllUsers(): void{//Вывод списка пользователей
    this.mServ.getAllUsers().subscribe(data => {
      this.isUsersList = true;
      this.isAdvertisments = true;
      this.isUserAdvs.isUserAdvsStatus = false;
      this.getLastPageForUserList();
      console.log(data);
      for( let i = 0; i < data.length; i++){
        let userItemComponent = this.componentFactoryResolver.resolveComponentFactory(UserComponent)// берется шаблон компонента
        let userItemComponentRef = this.book.createComponent(userItemComponent);// по шаблону создается элемент
        (<UserComponent>(userItemComponentRef.instance)).value = data[i]//данные берутся из сервиса
      }
    })
  }
  
  showParamsList(ev:Event): void{//выпадающее меню в списке параметров поиска
    document.addEventListener('click', function(ev){//При клике на любой элемент их выпадающего меню оно активируется
      let tg = ev.target as HTMLTextAreaElement;//так же при клике вне элемента, меню сворачивается
      let alwaysVisibleD = document.querySelector('.d-av') as HTMLTextAreaElement;
      let alwaysVisibleP = document.querySelector('.p-av') as HTMLTextAreaElement;
      if(alwaysVisibleD.contains(tg)){
          let btn = document.querySelector('img.d-btn') as HTMLTextAreaElement;
          btn.classList.toggle('menu-btn-rotate');
          let elems = document.querySelector('.date-sort-container>.always-visible')?.children;//изменение скругления рамок
          if(elems != undefined){
              for(let i = 0; i < elems?.length; i++){
              elems[i].classList.toggle('straight-borders');
            }
          }
          alwaysVisibleD.querySelector('.sort-params')?.classList.toggle('sort-params-hidden');
        }else if(alwaysVisibleP.contains(tg)){
           let btn = document.querySelector('img.p-btn') as HTMLTextAreaElement;
            btn.classList.toggle('menu-btn-rotate');
            let elems = document.querySelector('.price-sort-container>.always-visible')?.children;//изменение скругления рамок
            if(elems != undefined){
                for(let i = 0; i < elems?.length; i++){
                elems[i].classList.toggle('straight-borders');
              }
            }
           alwaysVisibleP.querySelector('.sort-params')?.classList.toggle('sort-params-hidden');
          }else{
            let elemsP = alwaysVisibleP?.children;
            if(elemsP != undefined){
              for(let i = 0; i < elemsP?.length; i++){
                elemsP[i].classList.remove('straight-borders');
              }
            }
            let elemsD = alwaysVisibleD?.children;
            if(elemsD != undefined){
              for(let i = 0; i < elemsD?.length; i++){
                elemsD[i].classList.remove('straight-borders');
              }
            }
            alwaysVisibleP.querySelector('img')?.classList.remove('menu-btn-rotate');
            alwaysVisibleD.querySelector('img')?.classList.remove('menu-btn-rotate');
            let sParams = document.querySelectorAll('.sort-params');
            for(let i = 0; i < sParams.length; i++){
              sParams[i].classList.add('sort-params-hidden');
            }
          }
    })
  }

  addValueToInput(ev:Event): void{//функция добавляет содержимое выбранного пункта в строку input, после чего убирает выпадающее меню
    let tg = ev.target as HTMLTextAreaElement;
    let alwaysVisibleD = document.querySelector('.d-av') as HTMLTextAreaElement;
    let alwaysVisibleP = document.querySelector('.p-av') as HTMLTextAreaElement;
    if(tg.classList.contains('d')){
      document.querySelector('.d-btn>img')?.classList.toggle('menu-btn-rotate');//поворот стрелочки
      this.sortForm.sortParamType = tg.innerHTML;//присвоение строке input значения из пункта списка
      this.mServ.setAllCategoryParamsSort(this.sortForm);
      alwaysVisibleD.querySelector('.sort-params')?.classList.toggle('sort-params-hidden');
      let elems = alwaysVisibleD?.children;//изменение скругления рамок
      if(elems != undefined){
          for(let i = 0; i < elems?.length; i++){
          elems[i].classList.toggle('straight-borders');
        }
      }
    }else if(tg.classList.contains('p')){
      document.querySelector('.p-btn>img')?.classList.toggle('menu-btn-rotate');//поворот стрелочки
      this.sortForm.sortType = tg.innerHTML;//присвоение строке input значения из пункта списка
      this.mServ.setAllCategoryParamsSort(this.sortForm);//Параметр указывает, какое именно поле было изменено
      alwaysVisibleP.querySelector('.sort-params')?.classList.toggle('sort-params-hidden');
      let elems = alwaysVisibleP?.children;//изменение скругления рамок
      if(elems != undefined){
          for(let i = 0; i < elems?.length; i++){
          elems[i].classList.toggle('straight-borders');
        }
      }
    }
    this.mServ.setAdvsType('params');
  }

  changeShowHiddenAdvsStatus(): void{
    // this.mServ.setAdvsType('user');
    this.mServ.setShowHiddenAdvsStatus(this.showHiddenAdvsField);
    // console.log(this.showHiddenAdvsField.showHiddenAdvs);
  }

  setParamsFromService(): void{//когда на сервисе параметры изменяются, данный метод подписывается на эти изменения и так же меняет свои параметры
      this.paginationForm.offset = this.mServ.getParams().offset;
      this.sortForm.sortType = this.mServ.changeParamToString(this.mServ.getParams().byDescending);
      if(this.mServ.getParams().sortByDate){
        this.sortForm.sortParamType = 'По дате';
      }else if(this.mServ.getParams().sortByCost){
        this.sortForm.sortParamType = 'По цене';
      }
      this.showHiddenAdvsField.showHiddenAdvs = this.mServ.getParams().showHiddenAdvs;
  }

  changePaginationOffset(ev: Event): void{//Изменение пагинации обьялвний при клике на стрелочки
    let tg = ev.target as HTMLTextAreaElement;
    if(tg.classList.contains('left-arrow') && (this.paginationForm.offset > 0)){
      this.paginationForm.offset -= this.mServ.getParams().limit;
    }else if(tg.classList.contains('right-arrow') && (this.currentPageNumber < this.lastPageNumber)){
      this.paginationForm.offset += this.mServ.getParams().limit;
    }
    this.currentPageNumber = this.paginationForm.offset/this.mServ.getParams().limit + 1;
    if(this.currentPageNumber != 1){
      this.prevPageNumber = this.currentPageNumber -1;
    }else{
      this.prevPageNumber = 0;
    }
    this.nextPageNumber = this.currentPageNumber +1;
    this.mServ.setAllParamsOffset(this.paginationForm);
  }

  goToThisPage(ev:Event):void {//Прямой переход на страницу, которую выбрал пользователь
    let tg = ev.target as HTMLTextAreaElement;
    if(tg.classList.contains('page-number-container')){
      this.currentPageNumber = Number(tg.children[0].innerHTML);
      if(this.currentPageNumber != 1){
        this.prevPageNumber = this.currentPageNumber -1;
      }else{
        this.prevPageNumber = 0;
      }
      console.log(this.currentPageNumber);
    }else{
      this.currentPageNumber = Number(tg.innerHTML);
      if(this.currentPageNumber != 1){
        this.prevPageNumber = this.currentPageNumber -1;
      }else{
        this.prevPageNumber = 0;
      }
      console.log(this.currentPageNumber);
    }
    this.nextPageNumber = this.currentPageNumber +1;
    this.paginationForm.offset = (this.currentPageNumber-1) * this.mServ.getParams().limit;
    this.mServ.setAllParamsOffset(this.paginationForm);
  }

  getAdminStatus(): boolean{//Проверка роли пользователя
    return this.service.getLoggedStatus() == 'admin';
  }

}