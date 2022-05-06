import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MiniImgComponent } from './mini-img.component';

describe('MiniImgComponent', () => {
  let component: MiniImgComponent;
  let fixture: ComponentFixture<MiniImgComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MiniImgComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MiniImgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
