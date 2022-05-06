import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FooterIdentComponent } from './footer-ident.component';

describe('FooterIdentComponent', () => {
  let component: FooterIdentComponent;
  let fixture: ComponentFixture<FooterIdentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FooterIdentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FooterIdentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
