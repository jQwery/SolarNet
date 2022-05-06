import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-aside-admin',
  templateUrl: './aside-admin.component.html',
  styleUrls: ['./aside-admin.component.css']
})
export class AsideAdminComponent implements OnInit {

  paramsForm: any = {
    sort: '',
    group: '',
  }

  printForm(): void{
    console.log(this.paramsForm)
  }

  clearParams(): void{//очистка всех выбранных параметров
    this.paramsForm.sort = '';
    this.paramsForm.group = '';
  }

  showCategoryList(ev:Event): void{//выпадающее меню в списке параметров поиска
    let tg = ev.target as HTMLTextAreaElement;
    document.querySelectorAll('.cat>img')[Number(tg.id)]?.classList.toggle('menu-btn-rotate');//поворот стрелочки
    // так как несколько одинаковых элементов, они беруться в зависимости от id элемента на котором был клик
    let elems = document.querySelector('.ibc'+ [Number(tg.id)])?.children;//изменение скругления рамок
    if(elems != undefined){
      for(let i = 0; i < elems?.length; i++){
        elems[i].classList.toggle('straight-borders');
      }
    }
    document.querySelector('.list'+ [Number(tg.id)])?.classList.toggle('list-hidden');//скрытие выпадающего списка
  }

  addValueToInput(ev:Event): void{//функция добавляет содержимое выбранного пункта в строку input, после чего убирает выпадающее меню
    let tg = ev.target as HTMLTextAreaElement;
    console.log(tg);
    let tgParent = tg.parentNode as HTMLTextAreaElement;
    document.querySelectorAll('.cat>img')[Number(tgParent.id)]?.classList.toggle('menu-btn-rotate');//поворот стрелочки
    if(tgParent.id == '0'){
      this.paramsForm.sort = tg.innerHTML;//присвоение строке input значения из пункта списка
      console.log("1", tgParent.id, tg.innerHTML)
    }else if(tgParent.id == '1'){
      this.paramsForm.group = tg.innerHTML;
      console.log("2", tgParent.id, tg.innerHTML)
    }

    document.querySelector('.list' + (Number(tgParent.id)))?.classList.toggle('list-hidden');//скрытие выпадающего списка
    let elems = document.querySelector('.ibc'+ (Number(tgParent.id)))?.children;//изменение скругления рамок
    if(elems != undefined){
      for(let i = 0; i < elems?.length; i++){
        elems[i].classList.toggle('straight-borders');
      }
    }
  }

  constructor() { }

  ngOnInit(): void {
  }

}
