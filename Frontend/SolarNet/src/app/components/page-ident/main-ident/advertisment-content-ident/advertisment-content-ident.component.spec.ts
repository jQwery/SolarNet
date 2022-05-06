import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertismentContentIdentComponent } from './advertisment-content-ident.component';

describe('AdvertismentContentIdentComponent', () => {
  let component: AdvertismentContentIdentComponent;
  let fixture: ComponentFixture<AdvertismentContentIdentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdvertismentContentIdentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvertismentContentIdentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
