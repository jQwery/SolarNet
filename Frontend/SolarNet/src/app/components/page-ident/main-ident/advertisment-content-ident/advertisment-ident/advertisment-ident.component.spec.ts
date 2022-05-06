import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertismentIdentComponent } from './advertisment-ident.component';

describe('AdvertismentIdentComponent', () => {
  let component: AdvertismentIdentComponent;
  let fixture: ComponentFixture<AdvertismentIdentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdvertismentIdentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvertismentIdentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
