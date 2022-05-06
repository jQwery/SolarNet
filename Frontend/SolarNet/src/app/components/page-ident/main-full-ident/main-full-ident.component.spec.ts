import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainFullIdentComponent } from './main-full-ident.component';

describe('MainFullIdentComponent', () => {
  let component: MainFullIdentComponent;
  let fixture: ComponentFixture<MainFullIdentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MainFullIdentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MainFullIdentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
