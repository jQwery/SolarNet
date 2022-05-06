import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { MainIdentRoutingRoutingModule } from './main-ident-routing/main-ident-routing-routing.module';
import { MainIdentGuard } from './main-ident-routing/main-ident.guard';

import { AdvertismentContentIdentComponent } from '../advertisment-content-ident/advertisment-content-ident.component';
import { AdvertismentIdentComponent } from '../advertisment-content-ident/advertisment-ident/advertisment-ident.component';
import { AsideIdentComponent } from '../aside-ident/aside-ident.component';
import { MainIdentComponent } from '../main-ident.component';
import { MainIdentServiceService } from '../main-ident-service.service';
import { AdvertismentAdminComponent } from 'src/app/components/page-admin/main-admin/advertisments-table-admin/advertisment-admin/advertisment-admin.component';
import { AdvertismentsTableAdminComponent } from 'src/app/components/page-admin/main-admin/advertisments-table-admin/advertisments-table-admin.component';
import { AsideAdminComponent } from 'src/app/components/page-admin/main-admin/aside-admin/aside-admin.component';
import { UserComponent } from '../user/user.component';


@NgModule({
  declarations: [
    MainIdentComponent,
    AdvertismentContentIdentComponent,
    AdvertismentIdentComponent,
    AsideIdentComponent,
    AsideAdminComponent,
    AdvertismentsTableAdminComponent,
    AdvertismentAdminComponent,
    UserComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    BrowserModule,
    MainIdentRoutingRoutingModule
  ],
  exports:[MainIdentComponent],
  providers:[MainIdentServiceService, MainIdentGuard],
  entryComponents:[
    AdvertismentContentIdentComponent,
    AdvertismentIdentComponent,
    AdvertismentsTableAdminComponent,
    AdvertismentAdminComponent,
    UserComponent
  ],
  bootstrap:[MainIdentComponent]
})
export class MainIdentModule { }
