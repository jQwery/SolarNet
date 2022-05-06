import { HttpClient, HttpEventType, HttpParams, HttpResponse } from '@angular/common/http';
import { Component, ComponentFactoryResolver, Inject, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MainIdentServiceService } from '../main-ident/main-ident-service.service';
import { NewAdvertismentService } from './new-advertisment.service';
import { AUTH_API_URL, STORE_API_URL } from '../../models/app-injection-token';
import { MiniImgComponent } from '../user-page/mini-img/mini-img.component';


@Component({
  selector: 'app-new-advertisment',
  templateUrl: './new-advertisment.component.html',
  styleUrls: ['./new-advertisment.component.css']
})
export class NewAdvertismentComponent implements OnInit {
  @ViewChild('newAdvForm') newAdvForm:any;
  @ViewChild('imgMini', {read: ViewContainerRef}) imgMini:any;

  cat: any;//Переменная для присвоения значения категории, так как то взято из таблицы, а не вводится вручную
  phLenght = 8;

  canRedirect: boolean = false;

  interval: any;

  changeAdvForm: any = {//Обьект редактирования данных формы
    categoryId: '',
    city: '',
    description: '',
    advertismentTitle: '',
    price: '',
    images: []
  }

  idCategory: string = '';

  categoriesNames: string[] = ['Транспорт','Вещи', 'Недвижимость', 'Электроника', 'Автомобили', 'Мотоциклы', 'Спецтехника', 'Водный транспорт', 'Одежда, обувь', 'Аксессуары', 'Для детей', 'Прочее',  'Квартиры', 'Дома', 'Комнаты', 'Гаражи',  'Телефоны', 'Ноутбуки', 'Комплектующие', 'Аксессуары'];


  print():void{
    console.log(this.changeAdvForm);
  }

  printNewAdvValues(): void{// функция при клике на кнопку опубликовать
    let date = new Date;
    let year = date.getFullYear();
    let month = String(date.getMonth() + 1);
    if(Number(month) < 10){
     month = '0' + month;
    }
    let day = String(date.getDate());
    if(Number(day) < 10){
      day = '0' + day;
    }
    this.newAdvForm.form.value.date = day + '.' + month + '.' + year;//Добавление даты публикации в обьект обьявления
    this.newAdvForm.form.value.photoLinks = this.nServ.imgList;// Добавление массива ссылок на фотографии
    console.log(this.newAdvForm.form.value);
  }

  showCategorySubList(ev:Event): void{// выпадающее меню для выбора категорий, позже сделать его по скрипту
    let tg = ev.currentTarget as HTMLTextAreaElement;
    tg.querySelector("ul.new-adv-category-sublist")?.classList.remove("none");
  }

  showCategoryMainList(ev:Event): void{//При клике на инпут с категорией, меню выпадает
    let tg = ev.target as HTMLTextAreaElement;
    document.querySelector('ul.new-adv-category-list')?.classList.remove("none");
  }

  closeCategorySublist(ev:Event): void{// при потере фокуса мыши, меню сворачивается
    let tg = ev.currentTarget as HTMLTextAreaElement;
    tg.querySelector("ul.new-adv-category-sublist")?.classList.add("none");
  }

  addValueToInput(ev:Event): void{//функция добавляет содержимое выбранного пункта в строку input
    let tg = ev.target as HTMLTextAreaElement;
    this.newAdvForm.form.value.category = tg.innerHTML;// присвоение переменной формы значения из таблицы
    this.changeAdvForm.categoryId = tg.innerHTML;//присвоение строке input значения из пункта списка
    this.idCategory = tg.id;
    document.querySelector('ul.new-adv-category-list')?.classList.add("none");

  }

  showHelpBar(ev:Event): void{// для каждого поля ввода на странице предусмотрена подсказка по наведнию на input
    let tg = ev.target as HTMLTextAreaElement;
    tg.nextElementSibling?.classList.remove('none');
  }

  closeHelpBar(ev:Event): void{// и когда фокус пропадает с input-а подсказка исчезает
    let tg = ev.target as HTMLTextAreaElement;
    tg.nextElementSibling?.classList.add('none');
  }

  addPhotoLinkInput(ev:Event): void{  //добавление полей для ввода ссылки на фотографию    
    this.phLenght--;// оставшееся количество возможных фотографий
    let tg = ev.currentTarget as HTMLTextAreaElement;
    let inp = tg.parentElement?.firstElementChild as HTMLInputElement; //получение input елемента путем поиска первого ребенка у родителя текущего элемента
    if((inp.value != '') && (this.phLenght >= 0)){ // если текущее поле заполнено, и еще есть возможные фотографии
      let nextElem = tg.parentElement?.nextSibling as HTMLTextAreaElement; //выбираем следующий элемент
      nextElem.classList.remove('none');    //убрать класс, скрывающий элемент
    }
  }

  getFormStatus(): boolean{
    return this.newAdvForm.dirty;
  }

  constructor(
    private nServ: NewAdvertismentService, 
    private mServ: MainIdentServiceService, 
    private router: Router, private aRouter: ActivatedRoute, 
    private http: HttpClient, 
    @Inject(STORE_API_URL) private storeUrl: string, 
    private componentFactoryResolver: ComponentFactoryResolver) 
  { 
    if(this.getPageType()){this.addAdv();}    //Проверка перехода, либо редактирование обьявления, либо его создание
  }

  getPageType(): boolean{
    return this.aRouter.snapshot.routeConfig?.path == 'changeAdvertisment/:id';
  }

  ngOnInit(): void {}

  submit(advData: any) {//Добавление обьявления
    advData.categoryId = this.idCategory;
    this.nServ.postData(advData).subscribe((data) => {
        console.log(data);
        this.router.navigate(['']);//направление на стартовую страницу
        this.canRedirect = true;
      },
      (error) => console.log(error)
    )
  }

  addAdv(): void{//функция добавления блока обьявления для его редактирования
    this.nServ.count = 0;
    this.nServ.getAdvValue(this.aRouter.snapshot.params['id']).subscribe(data => {//Получение данных обьявления по id
      console.log(data);
      // this.changeAdvForm = data;
      this.changeAdvForm.advertismentId = data.id;//Заполнение полей обьявления
      this.changeAdvForm.advertismentTitle = data.advertismentTitle;
      this.changeAdvForm.description = data.description;
      this.changeAdvForm.price = data.price;
      for(let i = 0; i< data.images.length; i++){
        let imgItemComponent = this.componentFactoryResolver.resolveComponentFactory(MiniImgComponent)// берется шаблон компонента
          let imgItemComponentRef = this.imgMini.createComponent(imgItemComponent);// по шаблону создается элемент
          (<MiniImgComponent>(imgItemComponentRef.instance)).value = data.images[i];//данные берутся из сервиса
          (<MiniImgComponent>(imgItemComponentRef.instance)).id = this.nServ.count;
          this.nServ.count++;
      }
      this.changeAdvForm.categoryId = this.categoriesNames[Number(data.categoryId)-1];
      this.changeAdvForm.city = data.city;
    })
  }

  changeAdv(): void{//Запрос изменения обьялвения
    this.changeAdvForm.images = this.nServ.imgList;
    this.changeAdvForm.categoryId = this.idCategory;
    console.log(this.changeAdvForm);
    this.nServ.changeAdvValue(this.changeAdvForm).subscribe((data) => {
      console.log(data);
      this.router.navigate(['']);
    },
    (error) =>{
      console.log(error);
    })
  }

  fileName = '';

  onFileSelected(event: any) {//метод для добавления фотографий в виде файлов
    const file:File[] = event.target?.files;

    this.newAdvForm.form.value.photoLinks = [];

    if (file.length != 0 && file.length < 8 && this.nServ.count < 8) {
      for(let i = 0; i < file.length; i++){
        this.fileName = file[i].name;
        const formData = new FormData();
        formData.append('Name', file[i].name);
        formData.append('Image', file[i]);
        // this.getImgs(formData);
        this.nServ.uploadFile(formData).subscribe(data => {//Создание миниатюр картинок
          console.log(data);
          let imgItemComponent = this.componentFactoryResolver.resolveComponentFactory(MiniImgComponent)// берется шаблон компонента
          let imgItemComponentRef = this.imgMini.createComponent(imgItemComponent);// по шаблону создается элемент
          (<MiniImgComponent>(imgItemComponentRef.instance)).value = data;//данные берутся из сервиса
          (<MiniImgComponent>(imgItemComponentRef.instance)).id = this.nServ.count;
          this.nServ.count++;
        });
      }
    }else{
      alert("Можно добавлять не более 8 картинок");
    }
  }

}
