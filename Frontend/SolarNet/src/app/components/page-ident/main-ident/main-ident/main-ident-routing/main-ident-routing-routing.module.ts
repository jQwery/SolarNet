import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainFullIdentComponent } from '../../../main-full-ident/main-full-ident.component';
import { NewAdvertismentComponent } from '../../../new-advertisment/new-advertisment.component';
import { UserPageComponent } from '../../../user-page/user-page.component';
import { MainIdentComponent } from '../../main-ident.component';
import { MainIdentDeactivateUserPageGuard } from './main-ident-deactivate-user-page.guard';
import { MainIdentDeactivateGuard } from './main-ident-deactivate.guard';
import { MainIdentGuard } from './main-ident.guard';

const mainIdentRoutes: Routes = [//прописаны маршруты для стартовой страницы
  {path: 'advertisment/:id', component: MainFullIdentComponent},//для перехода по обьявлению
  {path: 'createAdvertisment', component: NewAdvertismentComponent, canActivate:[MainIdentGuard], canDeactivate: [MainIdentDeactivateGuard]},
  {path: 'changeAdvertisment/:id', component: NewAdvertismentComponent},
  {path: 'userPage', component: UserPageComponent, canActivate:[MainIdentGuard], canDeactivate:[MainIdentDeactivateUserPageGuard]},
  {path: '', component: MainIdentComponent}//стартовая страница
];

@NgModule({
  imports: [RouterModule.forChild(mainIdentRoutes)],
  exports: [RouterModule]
})
export class MainIdentRoutingRoutingModule { }
