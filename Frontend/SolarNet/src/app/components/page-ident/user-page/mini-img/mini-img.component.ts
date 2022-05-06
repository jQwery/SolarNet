import { Component, OnInit, OnDestroy } from '@angular/core';
import { NewAdvertismentService } from '../../new-advertisment/new-advertisment.service';

@Component({
  selector: 'app-mini-img',
  templateUrl: './mini-img.component.html',
  styleUrls: ['./mini-img.component.css']
})
export class MiniImgComponent implements OnInit {

  value: any;

  id: any;

  constructor(private nServ: NewAdvertismentService) { }

  ngOnInit(): void {
    console.log(this.id);
    this.addDataToImgList();
  }

  addDataToImgList():void{//Добавление в массив картинок новой ссылки
     if(this.nServ.count > 8){
       alert("Можно добавлять не более 8 картинок");//Проверка на количество
     }else{
       this.nServ.addDataToImgList(this.id, this.value);
     }
  }
 
  deleteDataFromImgList(): void{//Удаление картинки из массива
    this.nServ.deleteDataFromImgList(this.id);
  }

  getId(): void{
    console.log(this.id);
  }

  destroyElem(ev:Event):void{//При нажатии на крестик, каринка удаляется из всех обьектов и массивов
    let tg = ev.target as HTMLTextAreaElement;//Удаляется и миниатюра
    let tgP;
    if (tg.classList.contains('img-c')){
      tgP = tg.parentElement?.parentElement as HTMLTextAreaElement;
    }else{
      tgP = tg.parentElement as HTMLTextAreaElement;
    }
    tgP.classList.add('none');
    this.deleteDataFromImgList();
  }

}
