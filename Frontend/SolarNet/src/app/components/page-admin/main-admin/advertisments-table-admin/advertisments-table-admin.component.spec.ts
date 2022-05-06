import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertismentsTableAdminComponent } from './advertisments-table-admin.component';

describe('AdvertismentsTableAdminComponent', () => {
  let component: AdvertismentsTableAdminComponent;
  let fixture: ComponentFixture<AdvertismentsTableAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdvertismentsTableAdminComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvertismentsTableAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
