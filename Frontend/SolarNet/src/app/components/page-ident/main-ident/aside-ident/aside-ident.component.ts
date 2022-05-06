import { Component, OnInit } from '@angular/core';
import { MainIdentServiceService } from '../main-ident-service.service';

@Component({
  selector: 'app-aside-ident',
  templateUrl: './aside-ident.component.html',
  styleUrls: ['./aside-ident.component.css']
})
export class AsideIdentComponent implements OnInit {
  paramsForm: any = {//обьект с параметрами
    idCategory: '',
    date: '',
    minimumCost: 0,
    maximumCost: 0,
    onlyWithPhoto: false,
    onlyWithComments: false,
    city: ''
  }

  idCategory: string = '';

  categoriesNames: string[] = ['Транспорт', 'Вещи', 'Недвижимость', 'Электроника', 'Автомобили', 'Мотоциклы', 'Спецтехника', 'Водный транспорт', 'Одежда, обувь', 'Аксессуары', 'Для детей', 'Прочее',  'Квартиры', 'Дома', 'Комнаты', 'Гаражи',  'Телефоны', 'Ноутбуки', 'Комплектующие', 'Аксессуары'];

  constructor(private mServ: MainIdentServiceService) { 
    
    this.mServ.getParamsInfo().subscribe(params =>{
      this.setParamsFromService(params);//Изменение параметров согласно сервису
      if(this.paramsForm.maximumCost == 0){
        this.getMaxCostS();//Если цена не введена вручную, то подьставляется максимально возможная
      }
    });

  }

  printForm(): void{
    console.log(this.paramsForm)
  }

  changeParamsForm(): void{//изменение параметров на сервисе
    this.paramsForm.idCategory = this.idCategory;
    this.mServ.setAllCategoryParamsAside(this.paramsForm);
    this.mServ.setAdvsType('params');
  }

  setParamsFromService(params:any): void{//когда на сервисе параметры изменяются, данный метод подписывается на эти изменения и так же меняет свои параметры
      this.paramsForm.idCategory = this.categoriesNames[Number(params.idCategory) - 1];
      this.paramsForm.date = params.date;
      this.paramsForm.minimumCost = params.minimumCost;
      this.paramsForm.maximumCost = params.maximumCost;
      // this.paramsForm.maximumCost = this.paramsForm.maximumCost;
      this.paramsForm.onlyWithPhoto = params.onlyWithPhoto;
      this.paramsForm.onlyWithComments = params.onlyWithComments;
      this.paramsForm.city = params.city;
  }

  clearParams(): void{//очистка всех выбранных параметров
    this.paramsForm.keyWords = '';
    this.paramsForm.minimumCost = 0;
    this.paramsForm.maximumCost = 0;
    this.mServ.getMaxCostS();
    this.paramsForm.idCategory = '';
    this.paramsForm.date = '';
    this.paramsForm.onlyWithPhoto = false;
    this.paramsForm.onlyWithComments = false;
    this.paramsForm.city = '';
    this.mServ.setAllCategoryParamsAside(this.paramsForm);
    this.mServ.setAdvsType('start');
  }

  clearAllParams(): void{//очистка параметров    
    this.mServ.clearAllCategoryParams();    
  }

  showCategoryList(ev:Event): void{//выпадающее меню в списке параметров поиска
    let tg = ev.target as HTMLTextAreaElement;
    document.querySelector('.cat>img')?.classList.toggle('menu-btn-rotate');//поворот стрелочки
    let elems = document.querySelector('.input-btn-continer')?.children;//изменение скругления рамок
    if(elems != undefined){
      for(let i = 0; i < elems?.length; i++){
        elems[i].classList.toggle('straight-borders');
      }
    }
    document.querySelector('.category-list')?.classList.toggle('category-list-hidden');//скрытие выпадающего списка
  }

  addValueToInput(ev:Event): void{//функция добавляет содержимое выбранного пункта в строку input, после чего убирает выпадающее меню
    let tg = ev.target as HTMLTextAreaElement;
    document.querySelector('.cat>img')?.classList.toggle('menu-btn-rotate');//поворот стрелочки
    this.paramsForm.idCategory = this.categoriesNames[Number(tg.id)-1];//присвоение строке input значения из пункта списка
    this.idCategory = tg.id;
    document.querySelector('.category-list')?.classList.add('category-list-hidden');//скрытие выпадающего списка
    let elems = document.querySelector('.input-btn-continer')?.children;//изменение скругления рамок
    if(elems != undefined){
      for(let i = 0; i < elems?.length; i++){
        elems[i].classList.remove('straight-borders');
      }
    }
  }

  openAside(ev: Event): void{//Для адаптива
    let tg = ev.target as HTMLTextAreaElement;
    document.querySelector('aside')?.classList.toggle('aside-open');//Сворачивание параметров
  }

  getMaxCostS(): void{//Получение макс. стоимости
      this.mServ.getMaxCost().subscribe(data => {
        this.paramsForm.maximumCost = data.cost + 1;
      })
  }

  ngOnInit(): void {
  }

}
