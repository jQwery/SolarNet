import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertismentAdminComponent } from './advertisment-admin.component';

describe('AdvertismentAdminComponent', () => {
  let component: AdvertismentAdminComponent;
  let fixture: ComponentFixture<AdvertismentAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdvertismentAdminComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvertismentAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
