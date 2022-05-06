import { Component, OnInit } from '@angular/core';
import { AppService } from 'src/app/app.service';

@Component({
  selector: 'app-main-ident',
  templateUrl: './main-ident.component.html',
  styleUrls: ['./main-ident.component.css']
})
export class MainIdentComponent implements OnInit {

  constructor(private appServ: AppService) { }

  getLoggedStatus(): string{
    return this.appServ.getLoggedStatus();
  }

  ngOnInit(): void {
  }

}
