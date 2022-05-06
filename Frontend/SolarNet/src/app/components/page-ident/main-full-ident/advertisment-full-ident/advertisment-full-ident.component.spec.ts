import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertismentFullIdentComponent } from './advertisment-full-ident.component';

describe('AdvertismentFullIdentComponent', () => {
  let component: AdvertismentFullIdentComponent;
  let fixture: ComponentFixture<AdvertismentFullIdentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdvertismentFullIdentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvertismentFullIdentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
