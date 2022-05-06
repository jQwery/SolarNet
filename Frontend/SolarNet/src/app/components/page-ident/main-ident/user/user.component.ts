import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MainIdentServiceService } from '../main-ident-service.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  value: any;

  mainImgLink: string = '';

  constructor(private mServ: MainIdentServiceService, private router: Router) { }

  ngOnInit(): void {
    console.log(this.value);
    this.createImgList();
  }

  createImgList(): void{
    if(this.value.avatarLink == 0){//Добавление ссылки к основному фото обьявления
      this.mainImgLink = 'https://cs.pikabu.ru/post_img/big/2013/08/24/1/1377296637_1500370441.png';
    }else{
      this.mainImgLink = this.value.avatarLink;
     }
  }

  addOtherUserAdvList(): void{//Вывод обьявлений пользователя
    this.mServ.otherUserId = this.value.id;
    this.mServ.otherUserName = this.value.name;
    this.mServ.setAdvsType('otherUser');
  }

  deleteUser(): void{//Бан пользователя
    this.router.navigate(['']);
    this.mServ.setAdvsType('usersData');
    if(this.value.role == 'Admin'){
      alert("Вы не имеете прав доступа для удаления администратора");
    }else{
      this.mServ.deleteUser(this.value.id).subscribe(data => {
        console.log(data);
      },error => {
        console.log(error.status);
      })
    }
  }

  showConfirmBar(): void{//Подтверждение действий
    let user = document.querySelector('.u' + this.value.id);
    user?.querySelector('.confirm-approve')?.classList.toggle('none');
  }
}
